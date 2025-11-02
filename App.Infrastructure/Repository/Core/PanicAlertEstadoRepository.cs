using App.Application.Interfaces.Core;
using App.Core.Entities.Core;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace App.Infrastructure.Repository.Core
{
    public class PanicAlertEstadoRepository : IPanicAlertEstadoRepository
    {
        private readonly IConfiguration _config;

        public PanicAlertEstadoRepository(IConfiguration configuration)
        {
            _config = configuration;
        }

        public async Task<IReadOnlyList<PanicAlertEstado>> GetAllAsync()
        {
            const string sql = "SELECT id, nombre FROM panic_alert_estado ORDER BY id";

            using (var connection = new NpgsqlConnection(_config.GetConnectionString(UnitOfWork.DefaultConnection)))
            {
                var items = await connection.QueryAsync<PanicAlertEstado>(sql);
                return items.AsList();
            }
        }

        public async Task<PanicAlertEstado?> GetByIdAsync(string id)
        {
            const string sql = "SELECT id, nombre FROM panic_alert_estado WHERE id = @id";

            var p = new DynamicParameters();
            p.Add("@id", id);

            using (var connection = new NpgsqlConnection(_config.GetConnectionString(UnitOfWork.DefaultConnection)))
            {
                return (await connection.QueryAsync<PanicAlertEstado>(sql, p)).FirstOrDefault();
            }
        }
    }
}
