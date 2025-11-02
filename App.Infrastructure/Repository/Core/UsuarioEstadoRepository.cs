using App.Application.Interfaces.Core;
using App.Core.Entities.Core;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace App.Infrastructure.Repository.Core
{
    public class UsuarioEstadoRepository : IUsuarioEstadoRepository
    {
        private readonly IConfiguration _config;

        public UsuarioEstadoRepository(IConfiguration configuration)
        {
            _config = configuration;
        }

        public async Task<IReadOnlyList<UsuarioEstado>> GetAllAsync()
        {
            const string sql = "SELECT id, nombre FROM usuario_estado ORDER BY id";

            using (var connection = new NpgsqlConnection(_config.GetConnectionString(UnitOfWork.DefaultConnection)))
            {
                var items = await connection.QueryAsync<UsuarioEstado>(sql);
                return items.AsList();
            }
        }

        public async Task<UsuarioEstado?> GetByIdAsync(string id)
        {
            const string sql = "SELECT id, nombre FROM usuario_estado WHERE id = @id";

            var p = new DynamicParameters();
            p.Add("@id", id);

            using (var connection = new NpgsqlConnection(_config.GetConnectionString(UnitOfWork.DefaultConnection)))
            {
                return (await connection.QueryAsync<UsuarioEstado>(sql, p)).FirstOrDefault();
            }
        }
    }
}
