using App.Application.Interfaces.Core;
using App.Core.Entities;
using App.Core.Entities.Core;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace App.Infrastructure.Repository.Core
{
    public class OcurrenciaTipoRepository : IOcurrenciaTipoRepository
    {
        private readonly IConfiguration _config;

        public OcurrenciaTipoRepository(IConfiguration configuration)
        {
            _config = configuration;
        }

        public async Task<Guid> AddAsync(OcurrenciaTipo entity)
        {
            const string sql = @"INSERT INTO ocurrencia_tipo (cliente_id, nombre, created_at, created_by)
                                 VALUES (@cliente_id, @nombre, @created_at, @created_by)
                                 RETURNING id";

            var p = new DynamicParameters();
            p.Add("@cliente_id", entity.cliente_id);
            p.Add("@nombre", entity.nombre);
            p.Add("@created_at", entity.created_at == null ? DateTimeOffset.UtcNow : entity.created_at);
            p.Add("@created_by", entity.created_by);

            using (var connection = new NpgsqlConnection(_config.GetConnectionString(UnitOfWork.DefaultConnection)))
            {
                await connection.OpenAsync();
                return await connection.ExecuteScalarAsync<Guid>(sql, p);
            }
        }

        public async Task AddOrUpdateAsync(OcurrenciaTipo entity)
        {
            const string upsert = @"INSERT INTO ocurrencia_tipo (id, cliente_id, nombre, created_at, created_by)
                                    VALUES (@id, @cliente_id, @nombre, @created_at, @created_by)
                                    ON CONFLICT (id) DO UPDATE
                                    SET cliente_id = EXCLUDED.cliente_id,
                                        nombre = EXCLUDED.nombre,
                                        updated_at = now(),
                                        updated_by = EXCLUDED.created_by;";

            var p = new DynamicParameters();
            p.Add("@id", entity.id == Guid.Empty ? Guid.NewGuid() : entity.id);
            p.Add("@cliente_id", entity.cliente_id);
            p.Add("@nombre", entity.nombre);
            p.Add("@created_at", entity.created_at == null ? DateTimeOffset.UtcNow : entity.created_at);
            p.Add("@created_by", entity.created_by);

            using (var connection = new NpgsqlConnection(_config.GetConnectionString(UnitOfWork.DefaultConnection)))
            {
                await connection.ExecuteAsync(upsert, p);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            const string sql = "UPDATE ocurrencia_tipo SET deleted_at = now() WHERE id = @id";

            var p = new DynamicParameters();
            p.Add("@id", id);

            using (var connection = new NpgsqlConnection(_config.GetConnectionString(UnitOfWork.DefaultConnection)))
            {
                await connection.ExecuteAsync(sql, p);
            }
        }

        public async Task<PaginaDatos<OcurrenciaTipo>> FindAsync(string? search, int page = 1, int pageSize = 20)
        {
            var sql = @"
                        SELECT count(*)
                        FROM ocurrencia_tipo ot
                        WHERE ot.deleted_at IS NULL
                        AND (@search IS NULL OR ot.nombre ILIKE '%' || @search || '%');

                        SELECT ot.*
                        FROM ocurrencia_tipo ot
                        WHERE ot.deleted_at IS NULL
                        AND (@search IS NULL OR ot.nombre ILIKE '%' || @search || '%')
                        ORDER BY ot.nombre
                        LIMIT @pageSize OFFSET @offset;";

            var offset = (page - 1) * pageSize;
            var parametros = new
            {
                search = string.IsNullOrWhiteSpace(search) ? null : search,
                offset,
                pageSize
            };

            using (var connection = new NpgsqlConnection(_config.GetConnectionString(UnitOfWork.DefaultConnection)))
            {
                using var multi = await connection.QueryMultipleAsync(sql, parametros);
                var total = await multi.ReadSingleAsync<int>();
                var data = (await multi.ReadAsync<OcurrenciaTipo>()).AsList();

                return new PaginaDatos<OcurrenciaTipo>
                {
                    total = total,
                    page = page,
                    pageSize = pageSize,
                    totalPages = (int)Math.Ceiling(total / (double)pageSize),
                    data = data
                };
            }
        }

        public async Task<OcurrenciaTipo?> GetByIdAsync(Guid id)
        {
            const string sql = "SELECT * FROM ocurrencia_tipo WHERE id = @id AND deleted_at IS NULL";

            var p = new DynamicParameters();
            p.Add("@id", id);

            using (var connection = new NpgsqlConnection(_config.GetConnectionString(UnitOfWork.DefaultConnection)))
            {
                return await connection.QueryFirstOrDefaultAsync<OcurrenciaTipo>(sql, p);
            }
        }

        public async Task UpdateAsync(OcurrenciaTipo entity)
        {
            const string sql = @"UPDATE ocurrencia_tipo SET
                                  cliente_id = @cliente_id,
                                  nombre = @nombre,
                                  updated_at = now(),
                                  updated_by = @updated_by
                                  WHERE id = @id";

            var p = new DynamicParameters();
            p.Add("@id", entity.id);
            p.Add("@cliente_id", entity.cliente_id);
            p.Add("@nombre", entity.nombre);
            p.Add("@updated_by", entity.updated_by);

            using (var connection = new NpgsqlConnection(_config.GetConnectionString(UnitOfWork.DefaultConnection)))
            {
                await connection.ExecuteAsync(sql, p);
            }
        }
    }
}
