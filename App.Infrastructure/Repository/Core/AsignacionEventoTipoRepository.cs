using App.Application.Interfaces.Core;
using App.Core.Entities.Core;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace App.Infrastructure.Repository.Core
{
    public class AsignacionEventoTipoRepository : IAsignacionEventoTipoRepository
    {
        private readonly IConfiguration _config;

        public AsignacionEventoTipoRepository(IConfiguration configuration)
        {
            _config = configuration;
        }

        public async Task<IReadOnlyList<AsignacionEventoTipo>> GetAllAsync()
        {
            const string sql = "SELECT id, nombre FROM asignacion_evento_tipo ORDER BY id";

            using (var connection = new NpgsqlConnection(_config.GetConnectionString(UnitOfWork.DefaultConnection)))
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

            using (var connection = new NpgsqlConnection(_config.GetConnectionString(UnitOfWork.DefaultConnection)))
            {
                return (await connection.QueryAsync<AsignacionEventoTipo>(sql, p)).FirstOrDefault();
            }
        }
    }
}
