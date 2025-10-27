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
        public async Task<string?> RegistrarAsync(UsuarioDto usuario)
        {
            try
            {
                string _query = @"INSERT INTO usuarios
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
                        var _usuarioId = await connection.ExecuteScalarAsync<string>(new CommandDefinition(_query, _parametros, transaction: sqlTran));

                        _query = "INSERT INTO usuarios_roles(usuario_id,rol_id) VALUES(@usuario_id,@rol_id)";

                        _parametros = new DynamicParameters();
                        _parametros.Add("@usuario_id", _usuarioId);
                        _parametros.Add("@rol_id", "U");

                        await connection.ExecuteAsync(new CommandDefinition(_query, _parametros, transaction: sqlTran));

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

            using (var connection = new NpgsqlConnection(_config.GetConnectionString()))
            {
                await connection.ExecuteAsync(_query, _parametros);
            }
        }
    }
}
