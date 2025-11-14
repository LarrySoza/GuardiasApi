using App.Application.Interfaces.Core;
using App.Core.Entities.Core;
using App.Infrastructure.Database;
using Dapper;

namespace App.Infrastructure.Repository.Core
{
    public class UsuarioRolRepository : IUsuarioRolRepository
    {
        private readonly IDbConnectionFactory _dbFactory;

        public UsuarioRolRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task AddAsync(Guid usuario_id, string rol_id)
        {
            const string sql = @"INSERT INTO usuario_rol (usuario_id, rol_id)
                                 VALUES (@usuario_id, @rol_id)
                                 ON CONFLICT (usuario_id, rol_id) DO NOTHING;";

            var p = new DynamicParameters();
            p.Add("@usuario_id", usuario_id);
            p.Add("@rol_id", rol_id);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                await connection.ExecuteAsync(sql, p);
            }
        }

        public async Task DeleteAsync(Guid usuario_id, string rol_id)
        {
            const string sql = "DELETE FROM usuario_rol WHERE usuario_id = @usuario_id AND rol_id = @rol_id";

            var p = new DynamicParameters();
            p.Add("@usuario_id", usuario_id);
            p.Add("@rol_id", rol_id);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                await connection.ExecuteAsync(sql, p);
            }
        }

        public async Task<IReadOnlyList<Rol>> GetAllAsync(Guid usuario_id)
        {
            const string sql = @"SELECT r.id, r.nombre
                                 FROM rol r
                                 JOIN usuario_rol ur ON ur.rol_id = r.id
                                 WHERE ur.usuario_id = @usuario_id
                                 ORDER BY r.id";

            var p = new DynamicParameters();
            p.Add("@usuario_id", usuario_id);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                var items = await connection.QueryAsync<Rol>(sql, p);
                return items.AsList();
            }
        }
    }
}
