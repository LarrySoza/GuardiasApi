using App.Application.Interfaces.Core;
using App.Core.Entities.Core;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace App.Infrastructure.Repository.Core
{
    public class IncidenteTipoRepository : IIncidenteTipoRepository
    {
        private readonly IConfiguration _config;

        public IncidenteTipoRepository(IConfiguration configuration)
        {
            _config = configuration;
        }

        public async Task<IReadOnlyList<IncidenteTipo>> GetAllAsync()
        {
            const string sql = "SELECT id, nombre FROM incidente_tipo ORDER BY nombre";

            using (var connection = new NpgsqlConnection(_config.GetConnectionString(UnitOfWork.DefaultConnection)))
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

            using (var connection = new NpgsqlConnection(_config.GetConnectionString(UnitOfWork.DefaultConnection)))
            {
                return (await connection.QueryAsync<IncidenteTipo>(sql, p)).FirstOrDefault();
            }
        }
    }
}
