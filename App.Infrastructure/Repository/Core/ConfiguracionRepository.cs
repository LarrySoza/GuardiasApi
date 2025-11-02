using App.Application.Interfaces.Core;
using App.Core.Entities.Core;
using App.Infrastructure.Database;
using Dapper;

namespace App.Infrastructure.Repository.Core
{
    public class ConfiguracionRepository : IConfiguracionRepository
    {
        private readonly IDbConnectionFactory _dbFactory;

        public ConfiguracionRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task AddAsync(Configuracion entity)
        {
            string _query = "INSERT INTO configuracion (id, valor) VALUES(@id, @valor)";

            var _parametros = new DynamicParameters();
            _parametros.Add("@id", entity.id);
            _parametros.Add("@valor", entity.valor);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                await connection.ExecuteAsync(_query, _parametros);
            }
        }

        public async Task AddOrUpdateAsync(Configuracion entity)
        {
            const string upsert = @"INSERT INTO configuracion (id, valor)
                                        VALUES (@id, @valor)
                                    ON CONFLICT (id) DO UPDATE
                                    SET valor = EXCLUDED.valor;";

            var p = new DynamicParameters();
            p.Add("@id", entity.id);
            p.Add("@valor", entity.valor);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                await connection.ExecuteAsync(upsert, p);
            }
        }

        public async Task DeleteAsync(string id)
        {
            string _query = "DELETE FROM configuracion WHERE id=@id";

            var _parametros = new DynamicParameters();
            _parametros.Add("@id", id);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                await connection.ExecuteAsync(_query, _parametros);
            }
        }

        public async Task<IReadOnlyList<Configuracion>> GetAllAsync()
        {
            string _query = "SELECT * FROM configuracion";

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                var items = await connection.QueryAsync<Configuracion>(_query);
                return items.AsList();
            }
        }

        public async Task<Configuracion?> GetByIdAsync(string id)
        {
            string _query = "SELECT * FROM configuracion WHERE id=@id";

            var _parametros = new DynamicParameters();
            _parametros.Add("@id", id);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                return (await connection.QueryAsync<Configuracion>(_query, _parametros)).FirstOrDefault();
            }
        }

        public async Task UpdateAsync(Configuracion entity)
        {
            string _query = "UPDATE configuracion SET valor = @valor WHERE id=@id";

            var _parametros = new DynamicParameters();
            _parametros.Add("@id", entity.id);
            _parametros.Add("@valor", entity.valor);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                await connection.ExecuteAsync(_query, _parametros);
            }
        }
    }
}
