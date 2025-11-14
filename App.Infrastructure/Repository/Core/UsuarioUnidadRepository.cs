using App.Application.Interfaces.Core;
using App.Core.Entities.Core;
using App.Infrastructure.Database;
using Dapper;

namespace App.Infrastructure.Repository.Core
{
    public class UsuarioUnidadRepository : IUsuarioUnidadRepository
    {
        private readonly IDbConnectionFactory _dbFactory;

        public UsuarioUnidadRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task AddAsync(Guid usuarioId, Guid unidadId)
        {
            const string sql = @"INSERT INTO usuario_unidad (usuario_id, unidad_id)
                                 VALUES (@usuario_id, @unidad_id)
                                 ON CONFLICT (usuario_id, unidad_id) DO NOTHING;";

            var p = new DynamicParameters();
            p.Add("@usuario_id", usuarioId);
            p.Add("@unidad_id", unidadId);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                await connection.ExecuteAsync(sql, p);
            }
        }

        public async Task DeleteAsync(Guid usuarioId, Guid unidadId)
        {
            const string sql = "DELETE FROM usuario_unidad WHERE usuario_id = @usuario_id AND unidad_id = @unidad_id";

            var p = new DynamicParameters();
            p.Add("@usuario_id", usuarioId);
            p.Add("@unidad_id", unidadId);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                await connection.ExecuteAsync(sql, p);
            }
        }

        public async Task<IReadOnlyList<Unidad>> GetAllAsync(Guid usuarioId)
        {
            const string sql = @"SELECT u.*
                                     FROM unidad u
                                     JOIN usuario_unidad uu ON uu.unidad_id = u.id
                                     WHERE uu.usuario_id = @usuario_id
                                     AND u.deleted_at IS NULL
                                     ORDER BY u.nombre";

            var p = new DynamicParameters();
            p.Add("@usuario_id", usuarioId);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                var items = await connection.QueryAsync<Unidad>(sql, p);
                return items.AsList();
            }
        }
    }
}
