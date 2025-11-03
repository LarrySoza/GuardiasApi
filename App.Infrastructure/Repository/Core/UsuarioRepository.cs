using App.Application.Interfaces.Core;
using App.Core.Entities;
using App.Core.Entities.Core;
using App.Infrastructure.Database;
using Dapper;

namespace App.Infrastructure.Repository.Core
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IDbConnectionFactory _dbFactory;

        public UsuarioRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task UpdatePasswordAsync(Guid id, string password)
        {
            string _query = @"UPDATE usuario SET
                                    contrasena_hash=@contrasena_hash,
                                    sello_seguridad=@sello_seguridad
                              WHERE 
                                    id=@id";

            var _parametros = new DynamicParameters();
            _parametros.Add("@contrasena_hash", Crypto.HashPassword(password));
            _parametros.Add("@sello_seguridad", Guid.NewGuid());
            _parametros.Add("@id", id);

            using (var connection = _dbFactory.CreateConnection())
            {
                await connection.ExecuteAsync(_query, _parametros);
            }
        }

        public async Task<Usuario?> GetByNameAsync(string name)
        {
            const string sql = @"
                SELECT *
                FROM usuario
                WHERE deleted_at IS NULL
                  AND lower(nombre_usuario) = lower(@name)
                LIMIT 1;";

            var p = new DynamicParameters();
            p.Add("@name", name);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                return await connection.QueryFirstOrDefaultAsync<Usuario>(sql, p);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            const string sql = "UPDATE usuario SET deleted_at = now() WHERE id = @id";
            var p = new DynamicParameters();
            p.Add("@id", id);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                await connection.ExecuteAsync(sql, p);
            }
        }

        // Implementación de la firma original heredada de ISearchRepository
        public Task<PaginaDatos<Usuario>> FindAsync(string? search, int page =1, int pageSize =20)
        {
            return FindAsync(search, page, pageSize, false);
        }

        public async Task<PaginaDatos<Usuario>> FindAsync(string? search, int page =1, int pageSize =20, bool includeRoles = false)
        {
            var offset = (page -1) * pageSize;
            var parametros = new
            {
                search = string.IsNullOrWhiteSpace(search) ? null : search,
                offset,
                pageSize
            };

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();

                if (!includeRoles)
                {
                    var sql = @"
                        SELECT count(*)
                        FROM usuario u
                        WHERE u.deleted_at IS NULL
                        AND (@search IS NULL
                        OR u.nombre_usuario ILIKE '%' || @search || '%'
                        OR u.email ILIKE '%' || @search || '%'
                        OR u.numero_documento ILIKE '%' || @search || '%');

                        SELECT u.*
                        FROM usuario u
                        WHERE u.deleted_at IS NULL
                        AND (@search IS NULL
                        OR u.nombre_usuario ILIKE '%' || @search || '%'
                        OR u.email ILIKE '%' || @search || '%'
                        OR u.numero_documento ILIKE '%' || @search || '%')
                        ORDER BY u.nombre_usuario
                        LIMIT @pageSize OFFSET @offset;";

                    using var multi = await connection.QueryMultipleAsync(sql, parametros);
                    var total = await multi.ReadSingleAsync<int>();
                    var data = (await multi.ReadAsync<Usuario>()).AsList();

                    return new PaginaDatos<Usuario>
                    {
                        total = total,
                        page = page,
                        pageSize = pageSize,
                        totalPages = (int)Math.Ceiling(total / (double)pageSize),
                        data = data
                    };
                }

                // includeRoles == true -> retornar usuarios con sus roles en una sola consulta
                var sqlWithRoles = @"
                    SELECT count(*)
                    FROM usuario u
                    WHERE u.deleted_at IS NULL
                    AND (@search IS NULL
                    OR u.nombre_usuario ILIKE '%' || @search || '%'
                    OR u.email ILIKE '%' || @search || '%'
                    OR u.numero_documento ILIKE '%' || @search || '%');

                    SELECT u.*, COALESCE(json_agg(json_build_object('id', r.id, 'nombre', r.nombre)) FILTER (WHERE r.id IS NOT NULL), '[]') as roles_json
                    FROM usuario u
                    LEFT JOIN usuario_rol ur ON ur.usuario_id = u.id AND ur.deleted_at IS NULL
                    LEFT JOIN rol r ON r.id = ur.rol_id
                    WHERE u.deleted_at IS NULL
                    AND (@search IS NULL
                    OR u.nombre_usuario ILIKE '%' || @search || '%'
                    OR u.email ILIKE '%' || @search || '%'
                    OR u.numero_documento ILIKE '%' || @search || '%')
                    GROUP BY u.id
                    ORDER BY u.nombre_usuario
                    LIMIT @pageSize OFFSET @offset;";

                using var multi2 = await connection.QueryMultipleAsync(sqlWithRoles, parametros);
                var total2 = await multi2.ReadSingleAsync<int>();

                // Leer cada registro como dynamic para capturar roles_json
                var rows = (await multi2.ReadAsync()).AsList();

                var usuarios = new List<Usuario>();
                foreach (var row in rows)
                {
                    // Mapear campos comunes a Usuario usando propiedades públicas.
                    var usuario = new Usuario();
                    usuario.SetId(row.id);
                    usuario.nombre_usuario = row.nombre_usuario;
                    usuario.email = row.email;
                    usuario.contrasena_hash = row.contrasena_hash;
                    usuario.sello_seguridad = row.sello_seguridad;
                    usuario.telefono = row.telefono;
                    usuario.tipo_documento_id = row.tipo_documento_id;
                    usuario.numero_documento = row.numero_documento;
                    usuario.estado = row.estado;
                    // No podemos asignar `id` ni campos de auditoría con setters protegidos desde aquí.

                    // Deserializar roles_json en una lista de Rol
                    var rolesJson = (string)row.roles_json;
                    try
                    {
                        var roles = System.Text.Json.JsonSerializer.Deserialize<List<Rol>>(rolesJson) ?? new List<Rol>();
                        usuario.roles = roles;
                    }
                    catch
                    {
                        // ignorar errores de deserialización y continuar
                    }

                    usuarios.Add(usuario);
                }

                return new PaginaDatos<Usuario>
                {
                    total = total2,
                    page = page,
                    pageSize = pageSize,
                    totalPages = (int)Math.Ceiling(total2 / (double)pageSize),
                    data = usuarios
                };
            }
        }

        public async Task<Usuario?> GetByIdAsync(Guid id)
        {
            const string sql = "SELECT * FROM usuario WHERE id = @id AND deleted_at IS NULL";
            var p = new DynamicParameters();
            p.Add("@id", id);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                return await connection.QueryFirstOrDefaultAsync<Usuario>(sql, p);
            }
        }

        public async Task UpdateAsync(Usuario entity)
        {
            const string sql = @"UPDATE usuario SET
                                    nombre_usuario = @nombre_usuario,
                                    email = @email,
                                    contrasena_hash = @contrasena_hash,
                                    sello_seguridad = @sello_seguridad,
                                    telefono = @telefono,
                                    tipo_documento_id = @tipo_documento_id,
                                    numero_documento = @numero_documento,
                                    estado = @estado,
                                    updated_at = now(),
                                    updated_by = @updated_by
                                 WHERE id = @id";

            var p = new DynamicParameters();
            p.Add("@id", entity.id);
            p.Add("@nombre_usuario", entity.nombre_usuario);
            p.Add("@email", entity.email);
            p.Add("@contrasena_hash", entity.contrasena_hash);
            p.Add("@sello_seguridad", entity.sello_seguridad);
            p.Add("@telefono", entity.telefono);
            p.Add("@tipo_documento_id", entity.tipo_documento_id);
            p.Add("@numero_documento", entity.numero_documento);
            p.Add("@estado", entity.estado);
            p.Add("@updated_by", entity.updated_by);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                await connection.ExecuteAsync(sql, p);
            }
        }

        public async Task<bool> ValidatePasswordAsync(Guid id, string password)
        {
            var _usuario = await GetByIdAsync(id);
            if (_usuario != null)
            {
                return Crypto.VerifyHashedPassword(_usuario.contrasena_hash!, password);
            }

            return false;
        }

        public async Task<Guid> AddAsync(Usuario usuario, List<Rol> roles)
        {
            const string sqlUser = @"INSERT INTO usuario
                                        (nombre_usuario,
                                        email, 
                                        contrasena_hash, 
                                        sello_seguridad, 
                                        telefono, 
                                        tipo_documento_id, 
                                        numero_documento, 
                                        estado, 
                                        created_at, 
                                        created_by)
                                    VALUES
                                        (@nombre_usuario, 
                                        @email, 
                                        @contrasena_hash, 
                                        @sello_seguridad, 
                                        @telefono, 
                                        @tipo_documento_id, 
                                        @numero_documento, 
                                        @estado, 
                                        @created_at, 
                                        @created_by)
                                    RETURNING id";

            const string sqlUsuarioRol = @"INSERT INTO usuario_rol 
                                                (usuario_id, 
                                                rol_id, 
                                                created_at) 
                                        VALUES 
                                                (@usuario_id, 
                                                @rol_id, 
                                                now())
                                        ON CONFLICT (usuario_id, rol_id) DO UPDATE
                                        SET deleted_at = NULL, updated_at = now();";

            var p = new DynamicParameters();
            p.Add("@nombre_usuario", usuario.nombre_usuario);
            p.Add("@email", usuario.email);
            p.Add("@contrasena_hash", usuario.contrasena_hash);
            p.Add("@sello_seguridad", usuario.sello_seguridad == Guid.Empty ? Guid.NewGuid() : usuario.sello_seguridad);
            p.Add("@telefono", usuario.telefono);
            p.Add("@tipo_documento_id", usuario.tipo_documento_id);
            p.Add("@numero_documento", usuario.numero_documento);
            p.Add("@estado", usuario.estado);
            p.Add("@created_at", usuario.created_at == null ? DateTimeOffset.UtcNow : usuario.created_at);
            p.Add("@created_by", usuario.created_by);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                using var tx = connection.BeginTransaction();
                try
                {
                    var id = await connection.ExecuteScalarAsync<Guid>(sqlUser, p, tx);

                    if (roles != null && roles.Count > 0)
                    {
                        foreach (var rol in roles)
                        {
                            var pr = new DynamicParameters();
                            pr.Add("@usuario_id", id);
                            pr.Add("@rol_id", rol.id);
                            await connection.ExecuteAsync(sqlUsuarioRol, pr, tx);
                        }
                    }

                    tx.Commit();
                    return id;
                }
                catch
                {
                    try { tx.Rollback(); } catch { }
                    throw;
                }
            }
        }
    }
}
