using App.Application.Interfaces.Core;
using App.Core.Entities.Core;
using App.Infrastructure.Database;
using Dapper;

namespace App.Infrastructure.Repository.Core
{
    public class AsignacionEventoTipoRepository : IAsignacionEventoTipoRepository
    {
        private readonly IDbConnectionFactory _dbFactory;

        public AsignacionEventoTipoRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<IReadOnlyList<AsignacionEventoTipo>> GetAllAsync()
        {
            const string sql = "SELECT id, nombre FROM asignacion_evento_tipo ORDER BY id";

            using (var connection = _dbFactory.CreateConnection())
            {
                var items = await connection.QueryAsync<AsignacionEventoTipo>(sql);
                return items.AsList();
            }
        }

        public async Task<AsignacionEventoTipo?> GetByIdAsync(string id)
        {
            const string sql = "SELECT id, nombre FROM asignacion_evento_tipo WHERE id = @id";

            var p = new DynamicParameters();
            p.Add("@id", id);

            using (var connection = _dbFactory.CreateConnection())
            {
                return (await connection.QueryAsync<AsignacionEventoTipo>(sql, p)).FirstOrDefault();
            }
        }
    }
}
