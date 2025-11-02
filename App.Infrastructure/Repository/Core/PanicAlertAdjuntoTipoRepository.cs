using App.Application.Interfaces.Core;
using App.Core.Entities.Core;
using App.Infrastructure.Database;
using Dapper;

namespace App.Infrastructure.Repository.Core
{
    public class PanicAlertAdjuntoTipoRepository : IPanicAlertAdjuntoTipoRepository
    {
        private readonly IDbConnectionFactory _dbFactory;

        public PanicAlertAdjuntoTipoRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<IReadOnlyList<PanicAlertAdjuntoTipo>> GetAllAsync()
        {
            const string sql = "SELECT id, nombre FROM panic_alert_adjunto_tipo ORDER BY id";

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                var items = await connection.QueryAsync<PanicAlertAdjuntoTipo>(sql);
                return items.AsList();
            }
        }

        public async Task<PanicAlertAdjuntoTipo?> GetByIdAsync(string id)
        {
            const string sql = "SELECT id, nombre FROM panic_alert_adjunto_tipo WHERE id = @id";

            var p = new DynamicParameters();
            p.Add("@id", id);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                return (await connection.QueryAsync<PanicAlertAdjuntoTipo>(sql, p)).FirstOrDefault();
            }
        }
    }
}
