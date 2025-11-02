using App.Application.Interfaces.Core;
using App.Core.Entities.Core;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace App.Infrastructure.Repository.Core
{
    public class TipoConfiguracionRepository : ITipoConfiguracionRepository
    {
        private readonly IConfiguration _config;

        public TipoConfiguracionRepository(IConfiguration configuration)
        {
            _config = configuration;
        }

        public async Task<IReadOnlyList<TipoConfiguracion>> GetAllAsync()
        {
            const string sql = "SELECT id, nombre FROM tipo_configuracion ORDER BY id";

            using (var connection = new NpgsqlConnection(_config.GetConnectionString(UnitOfWork.DefaultConnection)))
            {
                var items = await connection.QueryAsync<TipoConfiguracion>(sql);
                return items.AsList();
            }
        }

        public async Task<TipoConfiguracion?> GetByIdAsync(string id)
        {
            const string sql = "SELECT id, nombre FROM tipo_configuracion WHERE id = @id";

            var p = new DynamicParameters();
            p.Add("@id", id);

            using (var connection = new NpgsqlConnection(_config.GetConnectionString(UnitOfWork.DefaultConnection)))
            {
                return (await connection.QueryAsync<TipoConfiguracion>(sql, p)).FirstOrDefault();
            }
        }
    }
}
