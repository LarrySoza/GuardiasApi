using App.Application.Interfaces.Core;
using App.Core.Entities.Core;
using App.Infrastructure.Database;
using Dapper;

namespace App.Infrastructure.Repository.Core
{
    public class TipoDocumentoRepository : ITipoDocumentoRepository
    {
        private readonly IDbConnectionFactory _dbFactory;

        public TipoDocumentoRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<IReadOnlyList<TipoDocumento>> GetAllAsync()
        {
            const string sql = "SELECT id, nombre FROM tipo_documento ORDER BY id";

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                var items = await connection.QueryAsync<TipoDocumento>(sql);
                return items.AsList();
            }
        }

        public async Task<TipoDocumento?> GetByIdAsync(string id)
        {
            const string sql = "SELECT id, nombre FROM tipo_documento WHERE id = @id";

            var p = new DynamicParameters();
            p.Add("@id", id);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                return (await connection.QueryAsync<TipoDocumento>(sql, p)).FirstOrDefault();
            }
        }
    }
}
