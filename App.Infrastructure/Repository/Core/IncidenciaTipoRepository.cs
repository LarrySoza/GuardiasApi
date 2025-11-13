using App.Application.Interfaces.Core;
using App.Core.Entities.Core;
using App.Infrastructure.Database;
using Dapper;

namespace App.Infrastructure.Repository.Core
{
    public class IncidenciaTipoRepository : IIncidenciaTipoRepository
    {
        private readonly IDbConnectionFactory _dbFactory;

        public IncidenciaTipoRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<IReadOnlyList<IncidenciaTipo>> GetAllAsync()
        {
            const string sql = "SELECT id, nombre FROM incidencia_tipo ORDER BY nombre";

            using (var connection = _dbFactory.CreateConnection())
            {
                var items = await connection.QueryAsync<IncidenciaTipo>(sql);
                return items.AsList();
            }
        }

        public async Task<IncidenciaTipo?> GetByIdAsync(string id)
        {
            const string sql = "SELECT id, nombre FROM incidencia_tipo WHERE id = @id";

            var p = new DynamicParameters();
            p.Add("@id", id);

            using (var connection = _dbFactory.CreateConnection())
            {
                return (await connection.QueryAsync<IncidenciaTipo>(sql, p)).FirstOrDefault();
            }
        }
    }
}
