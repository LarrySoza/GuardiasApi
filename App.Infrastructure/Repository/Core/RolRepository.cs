using App.Application.Interfaces.Core;
using App.Core.Entities;
using App.Core.Entities.Core;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace App.Infrastructure.Repository.Core
{
    public class RolRepository : IRolRepository
    {
        private readonly IConfiguration _config;

        public RolRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<IReadOnlyList<Rol>> GetAllAsync()
        {
            const string sql = "SELECT id, codigo FROM rol ORDER BY id";

            using (var connection = new NpgsqlConnection(_config.GetConnectionString(UnitOfWork.DefaultConnection)))
            {
                var items = await connection.QueryAsync<Rol>(sql);
                return items.AsList();
            }
        }

        public async Task<Rol?> GetByIdAsync(string id)
        {
            const string sql = "SELECT id, codigo FROM rol WHERE id = @id";

            var p = new DynamicParameters();
            p.Add("@id", id);

            using (var connection = new NpgsqlConnection(_config.GetConnectionString(UnitOfWork.DefaultConnection)))
            {
                return (await connection.QueryAsync<Rol>(sql, p)).FirstOrDefault();
            }
        }
    }
}
