using App.Application.Interfaces.Core;
using App.Core.Entities;
using App.Core.Entities.Core;
using App.Infrastructure.Database;
using Dapper;

namespace App.Infrastructure.Repository.Core
{
    public class UnidadRepository : IUnidadRepository
    {
        private readonly IDbConnectionFactory _dbFactory;

        public UnidadRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<Guid> AddAsync(Unidad entity)
        {
            const string sql = @"INSERT INTO unidad
                        (cliente_id, unidad_id_padre, nombre, direccion, lat, lng, created_at, created_by)
                        VALUES
                        (@cliente_id, @unidad_id_padre, @nombre, @direccion, @lat, @lng, @created_at, @created_by)
                        RETURNING id";

            var p = new DynamicParameters();
            p.Add("@cliente_id", entity.cliente_id);
            p.Add("@unidad_id_padre", entity.unidad_id_padre);
            p.Add("@nombre", entity.nombre);
            p.Add("@direccion", entity.direccion);
            p.Add("@lat", entity.lat);
            p.Add("@lng", entity.lng);
            p.Add("@created_at", entity.created_at == null ? DateTimeOffset.UtcNow : entity.created_at);
            p.Add("@created_by", entity.created_by);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                using var tx = connection.BeginTransaction();
                try
                {
                    var id = await connection.ExecuteScalarAsync<Guid>(sql, p, tx);
                    tx.Commit();
                    return id;
                }
                catch
                {
                    try { tx.Rollback(); } catch { }
                    throw;
                }
            }
        }

        public async Task AddOrUpdateAsync(Unidad entity)
        {
            if (entity.id == null || (entity.id is Guid g && g == Guid.Empty))
            {
                await AddAsync(entity);
                return;
            }

            await UpdateAsync(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            const string sql = "UPDATE unidad SET deleted_at = now() WHERE id = @id";
            var p = new DynamicParameters();
            p.Add("@id", id);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                await connection.ExecuteAsync(sql, p);
            }
        }

        public async Task<PaginaDatos<Unidad>> FindAsync(string? search, int page = 1, int pageSize = 20)
        {
            var offset = (page - 1) * pageSize;
            var parametros = new
            {
                search = string.IsNullOrWhiteSpace(search) ? null : search,
                offset,
                pageSize
            };

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();

                var sql = @"
                    SELECT count(*)
                    FROM unidad u
                    WHERE u.deleted_at IS NULL
                    AND (@search IS NULL
                    OR u.nombre ILIKE '%' || @search || '%'
                    OR u.direccion ILIKE '%' || @search || '%');

                    SELECT u.*
                    FROM unidad u
                    WHERE u.deleted_at IS NULL
                    AND (@search IS NULL
                    OR u.nombre ILIKE '%' || @search || '%'
                    OR u.direccion ILIKE '%' || @search || '%')
                    ORDER BY u.nombre
                    LIMIT @pageSize OFFSET @offset;";

                using var multi = await connection.QueryMultipleAsync(sql, parametros);
                var total = await multi.ReadSingleAsync<int>();
                var data = (await multi.ReadAsync<Unidad>()).AsList();

                return new PaginaDatos<Unidad>
                {
                    total = total,
                    page = page,
                    pageSize = pageSize,
                    totalPages = (int)Math.Ceiling(total / (double)pageSize),
                    data = data
                };
            }
        }

        public async Task<PaginaDatos<Unidad>> FindAsync(Guid clienteId, string? search, int page = 1, int pageSize = 20)
        {
            var offset = (page - 1) * pageSize;
            var parametros = new
            {
                clienteId = clienteId,
                search = string.IsNullOrWhiteSpace(search) ? null : search,
                offset,
                pageSize
            };

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();

                var sql = @"
                    SELECT count(*)
                    FROM unidad u
                    WHERE u.deleted_at IS NULL
                    AND u.cliente_id = @clienteId
                    AND (@search IS NULL
                    OR u.nombre ILIKE '%' || @search || '%'
                    OR u.direccion ILIKE '%' || @search || '%');

                    SELECT u.*
                    FROM unidad u
                    WHERE u.deleted_at IS NULL
                    AND u.cliente_id = @clienteId
                    AND (@search IS NULL
                    OR u.nombre ILIKE '%' || @search || '%'
                    OR u.direccion ILIKE '%' || @search || '%')
                    ORDER BY u.nombre
                    LIMIT @pageSize OFFSET @offset;";

                using var multi = await connection.QueryMultipleAsync(sql, parametros);
                var total = await multi.ReadSingleAsync<int>();
                var data = (await multi.ReadAsync<Unidad>()).AsList();

                return new PaginaDatos<Unidad>
                {
                    total = total,
                    page = page,
                    pageSize = pageSize,
                    totalPages = (int)Math.Ceiling(total / (double)pageSize),
                    data = data
                };
            }
        }

        public async Task<Unidad?> GetByIdAsync(Guid id)
        {
            const string sql = "SELECT * FROM unidad WHERE id = @id AND deleted_at IS NULL";
            var p = new DynamicParameters();
            p.Add("@id", id);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                return await connection.QueryFirstOrDefaultAsync<Unidad>(sql, p);
            }
        }

        public async Task UpdateAsync(Unidad entity)
        {
            const string sql = @"UPDATE unidad SET
                            cliente_id = @cliente_id,
                            unidad_id_padre = @unidad_id_padre,
                            nombre = @nombre,
                            direccion = @direccion,
                            lat = @lat,
                            lng = @lng,
                            updated_at = now(),
                            updated_by = @updated_by
                            WHERE id = @id";

            var p = new DynamicParameters();
            p.Add("@id", entity.id);
            p.Add("@cliente_id", entity.cliente_id);
            p.Add("@unidad_id_padre", entity.unidad_id_padre);
            p.Add("@nombre", entity.nombre);
            p.Add("@direccion", entity.direccion);
            p.Add("@lat", entity.lat);
            p.Add("@lng", entity.lng);
            p.Add("@updated_by", entity.updated_by);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                using var tx = connection.BeginTransaction();
                try
                {
                    await connection.ExecuteAsync(sql, p, tx);
                    tx.Commit();
                }
                catch
                {
                    try { tx.Rollback(); } catch { }
                    throw;
                }
            }
        }
    }
}
