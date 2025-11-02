using App.Application.Interfaces.Core;
using App.Core.Entities.Core;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace App.Infrastructure.Repository.Core
{
    public class PanicAlertAdjuntoTipoRepository : IPanicAlertAdjuntoTipoRepository
    {
        private readonly IConfiguration _config;

        public PanicAlertAdjuntoTipoRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<IReadOnlyList<PanicAlertAdjuntoTipo>> GetAllAsync()
        {
            const string sql = "SELECT id, nombre FROM panic_alert_adjunto_tipo ORDER BY id";

            using (var connection = new NpgsqlConnection(_config.GetConnectionString(UnitOfWork.DefaultConnection)))
            {
                var items = await connection.QueryAsync<PanicAlertAdjuntoTipo>(sql);
                return items.AsList();
            }
        }

        public async Task<PanicAlertAdjuntoTipo?> GetByIdAsync(string id)
        {
            const string sql = "SELECT id, nombre FROM panic_alert_adjunto_tipo WHERE id = @id";

            var p = new DynamicParameters();
            p.Add("@id", id);

            using (var connection = new NpgsqlConnection(_config.GetConnectionString(UnitOfWork.DefaultConnection)))
            {
                return (await connection.QueryAsync<PanicAlertAdjuntoTipo>(sql, p)).FirstOrDefault();
            }
        }
    }
}
