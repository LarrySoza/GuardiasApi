using App.Application.Interfaces.Core;
using App.Core.Entities.Core;
using App.Infrastructure.Database;
using Dapper;
using System.Linq;

namespace App.Infrastructure.Repository.Core
{
    public class PanicAlertEstadoRepository : IPanicAlertEstadoRepository
    {
        private readonly IDbConnectionFactory _dbFactory;

        public PanicAlertEstadoRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<IReadOnlyList<PanicAlertEstado>> GetAllAsync()
        {
            const string sql = "SELECT id, nombre FROM panic_alert_estado ORDER BY id";

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                var items = await connection.QueryAsync<PanicAlertEstado>(sql);
                return items.AsList();
            }
        }

        public async Task<PanicAlertEstado?> GetByIdAsync(string id)
        {
            const string sql = "SELECT id, nombre FROM panic_alert_estado WHERE id = @id";

            var p = new DynamicParameters();
            p.Add("@id", id);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                return (await connection.QueryAsync<PanicAlertEstado>(sql, p)).FirstOrDefault();
            }
        }
    }
}
