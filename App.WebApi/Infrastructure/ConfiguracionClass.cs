using Dapper;
using App.WebApi.Entities;
using Npgsql;

namespace App.WebApi.Infrastructure
{
    public class ConfiguracionClass
    {
        private readonly IConfiguration _config;

        public ConfiguracionClass(IConfiguration config)
        {
            _config = config;
        }

        public Configuracion? Consultar(string? id)
        {
            string _query = "SELECT * FROM configuraciones WHERE tipo_configuracion_id=@tipo_configuracion_id";

            var _parametros = new DynamicParameters();
            _parametros.Add("@tipo_configuracion_id", id);

            using (var connection = new NpgsqlConnection(_config.GetConnectionString()))
            {
                return connection.Query<Configuracion>(_query, _parametros).FirstOrDefault();
            }
        }

        public async Task<Configuracion?> ConsultarAsync(string? id)
        {
            string _query = "SELECT * FROM configuraciones WHERE tipo_configuracion_id=@tipo_configuracion_id";

            var _parametros = new DynamicParameters();
            _parametros.Add("@tipo_configuracion_id", id);

            using (var connection = new NpgsqlConnection(_config.GetConnectionString()))
            {
                return (await connection.QueryAsync<Configuracion>(_query, _parametros)).FirstOrDefault();
            }
        }

        public async Task GuardarAsync(Configuracion configuracion)
        {
            var _configuracion = await ConsultarAsync(configuracion.tipo_configuracion_id);

            if (_configuracion == null)
            {
                await RegistrarAsync(configuracion);
            }
            else
            {
                await ActualizarAsync(configuracion);
            }
        }

        private async Task RegistrarAsync(Configuracion configuracion)
        {
            string _query = "INSERT INTO configuraciones VALUES(@tipo_configuracion_id,@valor)";

            var _parametros = new DynamicParameters();
            _parametros.Add("@tipo_configuracion_id", configuracion.tipo_configuracion_id);
            _parametros.Add("@valor", configuracion.valor);

            using (var connection = new NpgsqlConnection(_config.GetConnectionString()))
            {
                await connection.ExecuteAsync(_query, _parametros);
            }
        }

        private async Task ActualizarAsync(Configuracion configuracion)
        {
            string _query = "UPDATE configuraciones SET valor = @valor WHERE tipo_configuracion_id=@tipo_configuracion_id";

            var _parametros = new DynamicParameters();
            _parametros.Add("@tipo_configuracion_id", configuracion.tipo_configuracion_id);
            _parametros.Add("@valor", configuracion.valor);

            using (var connection = new NpgsqlConnection(_config.GetConnectionString()))
            {
                await connection.ExecuteAsync(_query, _parametros);
            }
        }
    }
}
