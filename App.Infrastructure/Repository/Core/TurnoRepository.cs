using App.Application.Interfaces.Core;
using App.Core.Entities.Core;
using App.Infrastructure.Database;
using Dapper;

namespace App.Infrastructure.Repository.Core
{
    public class TurnoRepository : ITurnoRepository
    {
        private readonly IDbConnectionFactory _dbFactory;

        public TurnoRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<IReadOnlyList<Turno>> GetAllAsync()
        {
            const string sql = "SELECT id, nombre FROM turno ORDER BY id";

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                var items = await connection.QueryAsync<Turno>(sql);
                return items.AsList();
            }
        }

        public async Task<Turno?> GetByIdAsync(int id)
        {
            const string sql = "SELECT id, nombre FROM turno WHERE id = @id";
            var p = new DynamicParameters();
            p.Add("@id", id);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                return (await connection.QueryAsync<Turno>(sql, p)).FirstOrDefault();
            }
        }
    }
}
