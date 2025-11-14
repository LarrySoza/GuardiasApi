using App.Application.Interfaces.Core;
using App.Core.Entities.Core;
using App.Infrastructure.Database;
using Dapper;

namespace App.Infrastructure.Repository.Core
{
    public class UsuarioPuestoRepository : IUsuarioPuestoRepository
    {
        private readonly IDbConnectionFactory _dbFactory;

        public UsuarioPuestoRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task AddAsync(Guid usuarioId, Guid puestoId)
        {
            const string sql = @"INSERT INTO usuario_puesto (usuario_id, puesto_id)
                                 VALUES (@usuario_id, @puesto_id)
                                 ON CONFLICT (usuario_id, puesto_id) DO NOTHING;";

            var p = new DynamicParameters();
            p.Add("@usuario_id", usuarioId);
            p.Add("@puesto_id", puestoId);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                await connection.ExecuteAsync(sql, p);
            }
        }

        public async Task DeleteAsync(Guid usuarioId, Guid puestoId)
        {
            const string sql = "DELETE FROM usuario_puesto WHERE usuario_id = @usuario_id AND puesto_id = @puesto_id";

            var p = new DynamicParameters();
            p.Add("@usuario_id", usuarioId);
            p.Add("@puesto_id", puestoId);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                await connection.ExecuteAsync(sql, p);
            }
        }

        public async Task<IReadOnlyList<Puesto>> GetAllAsync(Guid usuarioId)
        {
            const string sql = @"SELECT p.*
                                 FROM puesto p
                                 JOIN usuario_puesto up ON up.puesto_id = p.id
                                 WHERE up.usuario_id = @usuario_id
                                 AND p.deleted_at IS NULL
                                 ORDER BY p.nombre";

            var p = new DynamicParameters();
            p.Add("@usuario_id", usuarioId);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                var items = await connection.QueryAsync<Puesto>(sql, p);
                return items.AsList();
            }
        }
    }
}
