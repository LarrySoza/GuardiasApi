using App.Application.Interfaces.Core;
using App.Core.Entities.Core;
using App.Infrastructure.Database;
using Dapper;

namespace App.Infrastructure.Repository.Core
{
    public class UsuarioEstadoRepository : IUsuarioEstadoRepository
    {
        private readonly IDbConnectionFactory _dbFactory;

        public UsuarioEstadoRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<IReadOnlyList<UsuarioEstado>> GetAllAsync()
        {
            const string sql = "SELECT id, nombre FROM usuario_estado ORDER BY id";

            using (var connection = _dbFactory.CreateConnection())
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

            using (var connection = _dbFactory.CreateConnection())
            {
                return (await connection.QueryAsync<UsuarioEstado>(sql, p)).FirstOrDefault();
            }
        }
    }
}
