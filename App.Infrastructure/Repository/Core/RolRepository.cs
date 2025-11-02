using App.Application.Interfaces.Core;
using App.Core.Entities.Core;
using App.Infrastructure.Database;
using Dapper;

namespace App.Infrastructure.Repository.Core
{
    public class RolRepository : IRolRepository
    {
        private readonly IDbConnectionFactory _dbFactory;

        public RolRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<IReadOnlyList<Rol>> GetAllAsync()
        {
            const string sql = "SELECT id, codigo FROM rol ORDER BY id";

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                var items = await connection.QueryAsync<Rol>(sql);
                return items.AsList();
            }
        }

        public async Task<Rol?> GetByIdAsync(string id)
        {
            const string sql = "SELECT id, codigo FROM rol WHERE id = @id";

            var p = new DynamicParameters();
            p.Add("@id", id);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                return (await connection.QueryAsync<Rol>(sql, p)).FirstOrDefault();
            }
        }
    }
}
