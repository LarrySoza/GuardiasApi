using App.Application.Interfaces.Core;
using App.Core.Entities;
using App.Core.Entities.Core;
using App.Infrastructure.Database;
using Dapper;
using Npgsql;

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

        public async Task<Guid> AddAsync(Usuario entity)
        {
            const string sql = @"INSERT INTO usuario
                                 (nombre_usuario, email, contrasena_hash, sello_seguridad, telefono, tipo_documento, numero_documento, estado, created_at, created_by)
                                 VALUES
                                 (@nombre_usuario, @email, @contrasena_hash, @sello_seguridad, @telefono, @tipo_documento, @numero_documento, @estado, @created_at, @created_by)
                                 RETURNING id";

            var p = new DynamicParameters();
            p.Add("@nombre_usuario", entity.nombre_usuario);
            p.Add("@email", entity.email);
            p.Add("@contrasena_hash", entity.contrasena_hash);
            p.Add("@sello_seguridad", entity.sello_seguridad == Guid.Empty ? Guid.NewGuid() : entity.sello_seguridad);
            p.Add("@telefono", entity.telefono);
            p.Add("@tipo_documento", entity.tipo_documento);
            p.Add("@numero_documento", entity.numero_documento);
            p.Add("@estado", entity.estado);
            p.Add("@created_at", entity.created_at == null ? DateTimeOffset.UtcNow : entity.created_at);
            p.Add("@created_by", entity.created_by);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                return await connection.ExecuteScalarAsync<Guid>(sql, p);
            }
        }

        public async Task AddOrUpdateAsync(Usuario entity)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
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

        public async Task<PaginaDatos<Usuario>> FindAsync(string? search, int page = 1, int pageSize = 20)
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

            var offset = (page - 1) * pageSize;
            var parametros = new
            {
                search = string.IsNullOrWhiteSpace(search) ? null : search,
                offset,
                pageSize
            };

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
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
                                    tipo_documento = @tipo_documento,
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
            p.Add("@tipo_documento", entity.tipo_documento);
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
                return Crypto.VerifyHashedPassword(_usuario.contrasena_hash, password);
            }

            return false;
        }
    }
}
