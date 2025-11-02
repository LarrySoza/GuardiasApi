using App.Application.Interfaces.Core;
using App.Core.Entities.Core;
using App.Infrastructure.Database;
using Dapper;

namespace App.Infrastructure.Repository.Core
{
    public class IncidenteTipoRepository : IIncidenteTipoRepository
    {
        private readonly IDbConnectionFactory _dbFactory;

        public IncidenteTipoRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<IReadOnlyList<IncidenteTipo>> GetAllAsync()
        {
            const string sql = "SELECT id, nombre FROM incidente_tipo ORDER BY nombre";

            using (var connection = _dbFactory.CreateConnection())
            {
                var items = await connection.QueryAsync<IncidenteTipo>(sql);
                return items.AsList();
            }
        }

        public async Task<IncidenteTipo?> GetByIdAsync(string id)
        {
            const string sql = "SELECT id, nombre FROM incidente_tipo WHERE id = @id";

            var p = new DynamicParameters();
            p.Add("@id", id);

            using (var connection = _dbFactory.CreateConnection())
            {
                return (await connection.QueryAsync<IncidenteTipo>(sql, p)).FirstOrDefault();
            }
        }
    }
}
