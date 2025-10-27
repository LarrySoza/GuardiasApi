using App.WebApi.Entities;
using Dapper;
using Npgsql;
using System.Text.Json;

namespace App.WebApi.Infrastructure
{
    public class AreaClass
    {
        internal class AreaDbModel
        {
            public Guid area_id { get; set; }
            public string? nombre { get; set; }
            public string? coordenadas { get; set; } // <-- Dapper/Npgsql mapea JSONB a string
            public DateTime FechaCreacion { get; set; }
        }

        private readonly IConfiguration _config;

        public AreaClass(IConfiguration config)
        {
            _config = config;
        }

        public async Task<Guid> RegistrarAsync(AreaDto area)
        {
            string _query = @"INSERT INTO areas
                                (nombre, 
                                 coordenadas)
                           VALUES
                                (@nombre, 
                                 @coordenadas::jsonb)
                           RETURNING area_id";
            var _parametros = new DynamicParameters();
            _parametros.Add("@nombre", area.nombre);

            var coordenadasJson = area.coordenadas == null ? null : JsonSerializer.Serialize(area.coordenadas);

            _parametros.Add("@coordenadas", coordenadasJson);
            using (var connection = new NpgsqlConnection(_config.GetConnectionString()))
            {
                return await connection.ExecuteScalarAsync<Guid>(new CommandDefinition(_query, _parametros));
            }
        }

        public async Task<Area?> ConsultarPorNombreAsync(string nombre)
        {
            string _query = "SELECT * FROM areas WHERE nombre = @nombre";
            var _parametros = new DynamicParameters();
            _parametros.Add("@nombre", nombre);

            using (var connection = new NpgsqlConnection(_config.GetConnectionString()))
            {
                var areaDb = (await connection.QueryAsync<AreaDbModel>(_query, _parametros)).FirstOrDefault();

                return (areaDb == null) ? null : new Area
                {
                    area_id = areaDb.area_id,
                    nombre = areaDb.nombre!,
                    coordenadas = areaDb.coordenadas == null ? null : JsonSerializer.Deserialize<List<Coordenada>>(areaDb.coordenadas),
                    fecha_registro = areaDb.FechaCreacion
                };
            }
        }

        public async Task<Area?> ConsultarAsync(Guid areaId)
        {
            string _query = "SELECT * FROM areas WHERE area_id=@area_id";
            var _parametros = new DynamicParameters();
            _parametros.Add("@area_id", areaId);
            using (var connection = new NpgsqlConnection(_config.GetConnectionString()))
            {
                var areaDb = (await connection.QueryAsync<AreaDbModel>(_query, _parametros)).FirstOrDefault();

                return (areaDb == null) ? null : new Area
                {
                    area_id = areaDb.area_id,
                    nombre = areaDb.nombre!,
                    coordenadas = areaDb.coordenadas == null ? null : JsonSerializer.Deserialize<List<Coordenada>>(areaDb.coordenadas),
                    fecha_registro = areaDb.FechaCreacion
                };
            }
        }

        public async Task EliminarAsync(Guid areaId)
        {
            string _query = "DELETE FROM areas WHERE area_id=@area_id";
            var _parametros = new DynamicParameters();
            _parametros.Add("@area_id", areaId);
            using (var connection = new NpgsqlConnection(_config.GetConnectionString()))
            {
                await connection.ExecuteAsync(new CommandDefinition(_query, _parametros));
            }
        }

        public async Task<List<Area>> ListarAsync()
        {
            string _query = @"SELECT * FROM areas
                              ORDER BY nombre";
            using (var connection = new NpgsqlConnection(_config.GetConnectionString()))
            {
                var areasDb = await connection.QueryAsync<AreaDbModel>(new CommandDefinition(_query));
                return areasDb.Select(areaDb => new Area
                {
                    area_id = areaDb.area_id,
                    nombre = areaDb.nombre!,
                    coordenadas = areaDb.coordenadas == null ? null : JsonSerializer.Deserialize<List<Coordenada>>(areaDb.coordenadas),
                    fecha_registro = areaDb.FechaCreacion
                }).ToList();
            }
        }

        public async Task<List<Area>> BuscarAsync(string termino)
        {
            string _query = @"SELECT * FROM areas
                              WHERE nombre ILIKE @termino
                              ORDER BY nombre";
            var _parametros = new DynamicParameters();
            _parametros.Add("@termino", $"%{termino}%");
            using (var connection = new NpgsqlConnection(_config.GetConnectionString()))
            {
                var areasDb = await connection.QueryAsync<AreaDbModel>(new CommandDefinition(_query, _parametros));
                return areasDb.Select(areaDb => new Area
                {
                    area_id = areaDb.area_id,
                    nombre = areaDb.nombre!,
                    coordenadas = areaDb.coordenadas == null ? null : JsonSerializer.Deserialize<List<Coordenada>>(areaDb.coordenadas),
                    fecha_registro = areaDb.FechaCreacion
                }).ToList();
            }
        }

        public async Task ActualizarAsync(Guid areaId, AreaDto area)
        {
            string _query = @"UPDATE areas SET
                                nombre=@nombre,
                                coordenadas=@coordenadas
                              WHERE
                                area_id=@area_id";
            var _parametros = new DynamicParameters();
            _parametros.Add("@area_id", areaId);
            _parametros.Add("@nombre", area.nombre);
            _parametros.Add("@coordenadas", JsonSerializer.Serialize(area.coordenadas));
            using (var connection = new NpgsqlConnection(_config.GetConnectionString()))
            {
                await connection.ExecuteAsync(new CommandDefinition(_query, _parametros));
            }
        }

        public async Task<Guid> GuardarAsync(AreaDto area)
        {
            // Buscar por nombre
            var existente = await ConsultarPorNombreAsync(area.nombre);

            if (existente is not null)
            {
                await ActualizarAsync(existente.area_id, area);
                return existente.area_id;
            }
            else
            {
                return await RegistrarAsync(area);
            }
        }
    }
}
