using App.Application.Interfaces.Core;
using App.Core.Entities.Core;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace App.Infrastructure.Repository.Core
{
    public class AliveCheckEstadoRepository : IAliveCheckEstadoRepository
    {
        private readonly IConfiguration _config;

        public AliveCheckEstadoRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<IReadOnlyList<AliveCheckEstado>> GetAllAsync()
        {
            const string sql = "SELECT id, nombre FROM alive_check_estado ORDER BY id";

            using (var connection = new NpgsqlConnection(_config.GetConnectionString(UnitOfWork.DefaultConnection)))
            {
                var items = await connection.QueryAsync<AliveCheckEstado>(sql);
                return items.AsList();
            }
        }

        public async Task<AliveCheckEstado?> GetByIdAsync(string id)
        {
            const string sql = "SELECT id, nombre FROM alive_check_estado WHERE id = @id";

            var p = new DynamicParameters();
            p.Add("@id", id);

            using (var connection = new NpgsqlConnection(_config.GetConnectionString(UnitOfWork.DefaultConnection)))
            {
                return (await connection.QueryAsync<AliveCheckEstado>(sql, p)).FirstOrDefault();
            }
        }
    }
}
