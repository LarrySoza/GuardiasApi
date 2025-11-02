using App.Application.Interfaces.Core;
using App.Core.Entities.Core;
using App.Infrastructure.Database;
using Dapper;

namespace App.Infrastructure.Repository.Core
{
    public class AliveCheckEstadoRepository : IAliveCheckEstadoRepository
    {
        private readonly IDbConnectionFactory _dbFactory;

        public AliveCheckEstadoRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<IReadOnlyList<AliveCheckEstado>> GetAllAsync()
        {
            const string sql = "SELECT id, nombre FROM alive_check_estado ORDER BY id";

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                var items = await connection.QueryAsync<AliveCheckEstado>(sql);
                return items.AsList();
            }
        }

        public async Task<AliveCheckEstado?> GetByIdAsync(string id)
        {
            const string sql = "SELECT id, nombre FROM alive_check_estado WHERE id = @id";

            var p = new DynamicParameters();
            p.Add("@id", id);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                return (await connection.QueryAsync<AliveCheckEstado>(sql, p)).FirstOrDefault();
            }
        }
    }
}
