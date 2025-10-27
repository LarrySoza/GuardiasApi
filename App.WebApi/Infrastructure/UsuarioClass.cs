using Dapper;
using App.WebApi.Entities;
using Npgsql;
using System.Text.Json;

namespace App.WebApi.Infrastructure
{
    public class UsuarioClass
    {
        private readonly IConfiguration _config;

        public UsuarioClass(IConfiguration config)
        {
            _config = config;
        }

        public async Task<List<Rol>> ListarRolesAsync(Guid usuarioId)
        {
            string _query = @"SELECT 
	                            r.*
                            FROM 
	                            usuarios_roles u
                            INNER JOIN
	                            roles r
                            ON
	                            u.rol_id = r.rol_id
                            WHERE
	                            u.usuario_id= @usuario_id";

            var _parametros = new DynamicParameters();
            _parametros.Add("@usuario_id", usuarioId);

            using (var connection = new NpgsqlConnection(_config.GetConnectionString()))
            {
                return (await connection.QueryAsync<Rol>(new CommandDefinition(_query, _parametros))).ToList();
            }
        }

        /// <summary>
        /// Registrar un usuario y devuelve el id asignado
        /// </summary>
        /// <param name="usuario">Informacion del usuario</param>
        /// <returns></returns>
        public async Task<string?> RegistrarAsync(UsuarioRegistroDto usuario)
        {
            try
            {
                string _queryUsuario = @"INSERT INTO usuarios
	                                (nombre_usuario, 
	                                 clave_hash, 
	                                 correo,
                                     correo_confirmado,
                                     sello_seguridad)
                              VALUES
	                                (@nombre_usuario, 
	                                 @clave_hash, 
	                                 @correo,
                                     @correo_confirmado,
                                     @sello_seguridad)
                              RETURNING usuario_id";

                string _queryPerfil = @"INSERT INTO usuarios_perfiles
                                    (usuario_id, 
                                     nombres, 
                                     apellidos, 
                                     tipo_documento, 
                                     numero_documento, 
                                     area_id)
                              VALUES
                                    (@usuario_id, 
                                     @nombres, 
                                     @apellidos, 
                                     @tipo_documento, 
                                     @numero_documento, 
                                     @area_id)";

                var _queryRol = "INSERT INTO usuarios_roles(usuario_id,rol_id) VALUES(@usuario_id,@rol_id)";

                var _parametros = new DynamicParameters();
                _parametros.Add("@nombre_usuario", usuario.nombre_usuario);
                _parametros.Add("@clave_hash", Crypto.HashPassword(usuario.clave!));
                _parametros.Add("@correo");
                _parametros.Add("@correo_confirmado", false);
                _parametros.Add("@sello_seguridad", Guid.NewGuid().ToString());

                using (var connection = new NpgsqlConnection(_config.GetConnectionString()))
                {
                    await connection.OpenAsync();

                    using (var sqlTran = connection.BeginTransaction())
                    {
                        var _usuarioId = await connection.ExecuteScalarAsync<string>(new CommandDefinition(_queryUsuario, _parametros, transaction: sqlTran));

                        _parametros = new DynamicParameters();
                        _parametros.Add("@usuario_id", _usuarioId);
                        _parametros.Add("@rol_id", "U");

                        await connection.ExecuteAsync(new CommandDefinition(_queryRol, _parametros, transaction: sqlTran));

                        _parametros = new DynamicParameters();
                        _parametros.Add("@usuario_id", _usuarioId);
                        _parametros.Add("@nombres", usuario.nombres);
                        _parametros.Add("@apellidos", usuario.apellidos);
                        _parametros.Add("@tipo_documento", usuario.tipo_documento);
                        _parametros.Add("@numero_documento", usuario.numero_documento);
                        _parametros.Add("@area_id", usuario.area_id);

                        await connection.ExecuteAsync(new CommandDefinition(_queryPerfil, _parametros, transaction: sqlTran));

                        await sqlTran.CommitAsync();

                        return _usuarioId;
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                if (ex.SqlState == "23505")
                {
                    throw new Exception($"El usuario '{usuario.nombre_usuario}' ya existe");
                }
                else
                {
                    throw;
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task ActualizarPerfilAsync(Guid usuarioId, UsuarioActualizaPerfilDto perfil)
        {
            string _query = @"UPDATE usuarios_perfiles SET
                                    nombres=@nombres,
                                    apellidos=@apellidos,
                                    tipo_documento=@tipo_documento,
                                    numero_documento=@numero_documento,
                                    area_id=@area_id,
                                    fecha_actualizacion=NOW()
                              WHERE 
                                    usuario_id=@usuario_id";
            var _parametros = new DynamicParameters();
            _parametros.Add("@nombres", perfil.nombres);
            _parametros.Add("@apellidos", perfil.apellidos);
            _parametros.Add("@tipo_documento", perfil.tipo_documento);
            _parametros.Add("@numero_documento", perfil.numero_documento);
            _parametros.Add("@area_id", perfil.area_id);
            _parametros.Add("@usuario_id", usuarioId);

            using (var connection = new NpgsqlConnection(_config.GetConnectionString()))
            {
                await connection.ExecuteAsync(_query, _parametros);
            }
        }

        public async Task<bool> ValidarClave(Guid usuarioId, string clave)
        {
            var _usuario = await ConsultarPorIdAsync(usuarioId);
            if (_usuario != null)
            {
                return Crypto.VerifyHashedPassword(_usuario.clave_hash, clave);
            }

            return false;
        }

        public async Task<Usuario?> ConsultarPorIdAsync(Guid usuarioId)
        {
            string _query = "SELECT * FROM usuarios WHERE usuario_id=@usuario_id";
            var _parametros = new DynamicParameters();
            _parametros.Add("@usuario_id", usuarioId);

            using (var connection = new NpgsqlConnection(_config.GetConnectionString()))
            {
                return await connection.QueryFirstOrDefaultAsync<Usuario>(_query, _parametros);
            }
        }

        public async Task<Usuario?> ConsultarPorNombreUsuarioAsync(string nombreUsuario)
        {
            string _query = "SELECT * FROM usuarios WHERE nombre_usuario=@nombre_usuario";
            var _parametros = new DynamicParameters();
            _parametros.Add("@nombre_usuario", nombreUsuario);

            using (var connection = new NpgsqlConnection(_config.GetConnectionString()))
            {
                return await connection.QueryFirstOrDefaultAsync<Usuario>(_query, _parametros);
            }
        }

        public Usuario? ConsultarPorId(Guid usuarioId)
        {
            string _query = @"SELECT * FROM usuarios WHERE usuario_id=@usuarioId";
            var _parametros = new DynamicParameters();
            _parametros.Add("@usuarioId", usuarioId);

            using (var connection = new NpgsqlConnection(_config.GetConnectionString()))
            {
                return connection.QueryFirstOrDefault<Usuario>(_query, _parametros);
            }
        }

        public async Task ActualizarClaveAsync(Guid usuarioId, string clave)
        {
            string _query = @"UPDATE usuarios SET
                                    clave_hash=@clave_hash,
                                    sello_seguridad=@sello_seguridad
                              WHERE 
                                    usuario_id=@usuario_id";

            var _parametros = new DynamicParameters();
            _parametros.Add("@clave_hash", Crypto.HashPassword(clave));
            _parametros.Add("@sello_seguridad", Guid.NewGuid().ToString());
            _parametros.Add("@usuario_id", usuarioId);

            using (var connection = new NpgsqlConnection(_config.GetConnectionString()))
            {
                await connection.ExecuteAsync(_query, _parametros);
            }
        }

        public async Task CambiarActivoAsync(Guid usuarioId)
        {
            string _query = @"UPDATE usuarios SET
                                    activo=not activo
                              WHERE 
                                    usuario_id=@usuario_id";

            var _parametros = new DynamicParameters();
            _parametros.Add("@usuario_id", usuarioId);
            _parametros.Add("@sello_seguridad", Guid.NewGuid().ToString());

            using (var connection = new NpgsqlConnection(_config.GetConnectionString()))
            {
                await connection.ExecuteAsync(_query, _parametros);
            }
        }

        public async Task<PaginaDatosModel<VwUsuarioPerfil>> BuscarAsync(string? search, int page = 1, int pageSize = 20)
        {
            using (var dbClientes = new NpgsqlConnection(_config.GetConnectionString()))
            {
                var _sql = @"
                             SELECT count(*)
                             FROM usuarios u
                             JOIN usuarios_perfiles up ON u.usuario_id = up.usuario_id
                             LEFT JOIN areas a ON up.area_id = a.area_id
                             WHERE (@search IS NULL
                                OR up.nombres ILIKE '%' || @search || '%'
                                OR up.apellidos ILIKE '%' || @search || '%'
                                OR up.numero_documento ILIKE '%' || @search || '%');

                             SELECT
                                u.usuario_id,
                                u.nombre_usuario,
                                up.nombres,
                                up.apellidos,
                                up.tipo_documento,
                                up.numero_documento,
                                up.area_id,
                                up.fecha_actualizacion,
                                a.nombre AS nombre_area
                             FROM usuarios u
                             JOIN usuarios_perfiles up ON u.usuario_id = up.usuario_id
                             LEFT JOIN areas a ON up.area_id = a.area_id
                             WHERE (@search IS NULL
                                OR up.nombres ILIKE '%' || @search || '%'
                                OR up.apellidos ILIKE '%' || @search || '%'
                                OR up.numero_documento ILIKE '%' || @search || '%')
                                ORDER BY up.apellidos
                                LIMIT @pageSize OFFSET @offset;";

                var offset = (page - 1) * pageSize;

                var parametros = new
                {
                    search = string.IsNullOrWhiteSpace(search) ? null : search,
                    offset,
                    pageSize
                };

                using var multi = await dbClientes.QueryMultipleAsync(_sql, parametros);

                var total = await multi.ReadSingleAsync<int>();
                var data = await multi.ReadAsync<VwUsuarioPerfil>();

                return new PaginaDatosModel<VwUsuarioPerfil>()
                {
                    total = total,
                    page = page,
                    pageSize = pageSize,
                    totalPages = (int)Math.Ceiling(total / (double)pageSize),
                    data = data.ToList()
                };
            }
        }
    }
}
