using App.Application.Interfaces.Core;
using App.Core.Entities;
using App.Core.Entities.Core;
using App.Infrastructure.Database;
using Dapper;

namespace App.Infrastructure.Repository.Core
{
    public class PanicAlertRepository : IPanicAlertRepository
    {
        private readonly IDbConnectionFactory _dbFactory;

        public PanicAlertRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<Guid> AddAsync(PanicAlert entity)
        {
            const string insertSql = @"INSERT INTO panic_alert
                                     (sesion_usuario_id, fecha_hora, lat, lng, mensaje, estado_id, created_at, created_by)
                                     VALUES (@sesion_usuario_id, @fecha_hora, @lat, @lng, @mensaje, @estado_id, @created_at, @created_by)
                                     RETURNING id";

            var p = new DynamicParameters();
            p.Add("@sesion_usuario_id", entity.sesion_usuario_id);
            p.Add("@fecha_hora", entity.fecha_hora == default ? DateTimeOffset.UtcNow : entity.fecha_hora);
            p.Add("@lat", entity.lat);
            p.Add("@lng", entity.lng);
            p.Add("@mensaje", entity.mensaje);
            p.Add("@estado_id", entity.estado_id);
            p.Add("@created_at", entity.created_at == null ? DateTimeOffset.UtcNow : entity.created_at);
            p.Add("@created_by", entity.created_by);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                using var tx = connection.BeginTransaction();
                try
                {
                    var id = await connection.ExecuteScalarAsync<Guid>(insertSql, p, tx);
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

        public async Task DeleteAsync(Guid id)
        {
            const string sql = "UPDATE panic_alert SET deleted_at = now() WHERE id = @id";
            var p = new DynamicParameters();
            p.Add("@id", id);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                await connection.ExecuteAsync(sql, p);
            }
        }

        public async Task<IReadOnlyList<PanicAlert>> GetAllAsync(Guid sesionId)
        {
            const string sql = @"SELECT *
                                FROM panic_alert
                                WHERE sesion_usuario_id = @sesionId
                                AND deleted_at IS NULL
                                ORDER BY fecha_hora DESC";

            var p = new DynamicParameters();
            p.Add("@sesionId", sesionId);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                var result = (await connection.QueryAsync<PanicAlert>(sql, p)).AsList();
                return result;
            }
        }

        public async Task<PanicAlert?> GetByIdAsync(Guid id)
        {
            const string sql = "SELECT * FROM panic_alert WHERE id = @id AND deleted_at IS NULL";
            var p = new DynamicParameters();
            p.Add("@id", id);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                return await connection.QueryFirstOrDefaultAsync<PanicAlert>(sql, p);
            }
        }

        public async Task<PaginaDatos<PanicAlert>> GetPagedByClienteIdAsync(Guid clienteId, string? estadoId, int page = 1, int pageSize = 20, DateOnly? date = null)
        {
            var offset = (page - 1) * pageSize;
            var sql = @"
                         SELECT count(*)
                         FROM panic_alert p
                         INNER JOIN sesion_usuario s ON p.sesion_usuario_id = s.id
                         WHERE p.deleted_at IS NULL
                         AND s.deleted_at IS NULL
                         AND s.cliente_id = @clienteId
                         AND (@estadoId IS NULL OR p.estado_id = @estadoId)
                         AND (@date IS NULL OR DATE(p.fecha_hora) = @date);

                         SELECT p.*
                         FROM panic_alert p
                         INNER JOIN sesion_usuario s ON p.sesion_usuario_id = s.id
                         WHERE p.deleted_at IS NULL
                         AND s.deleted_at IS NULL
                         AND s.cliente_id = @clienteId
                         AND (@estadoId IS NULL OR p.estado_id = @estadoId)
                         AND (@date IS NULL OR DATE(p.fecha_hora) = @date)
                         ORDER BY p.fecha_hora DESC
                         LIMIT @pageSize OFFSET @offset;";

            var parametros = new DynamicParameters();
            parametros.Add("@clienteId", clienteId);
            parametros.Add("@estadoId", estadoId);
            parametros.Add("@date", date.HasValue ? date.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null);
            parametros.Add("@offset", offset);
            parametros.Add("@pageSize", pageSize);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                using var multi = await connection.QueryMultipleAsync(sql, parametros);
                var total = await multi.ReadSingleAsync<int>();
                var data = (await multi.ReadAsync<PanicAlert>()).AsList();

                return new PaginaDatos<PanicAlert>
                {
                    total = total,
                    page = page,
                    pageSize = pageSize,
                    totalPages = (int)Math.Ceiling(total / (double)pageSize),
                    data = data
                };
            }
        }

        public async Task<PaginaDatos<PanicAlert>> GetPagedByUnidadIdAsync(Guid unidadId, string? estadoId, int page = 1, int pageSize = 20, DateOnly? date = null)
        {
            var offset = (page - 1) * pageSize;
            var sql = @"
                     SELECT count(*)
                     FROM panic_alert p
                     INNER JOIN sesion_usuario s ON p.sesion_usuario_id = s.id
                     WHERE p.deleted_at IS NULL
                     AND s.deleted_at IS NULL
                     AND s.unidad_id = @unidadId
                     AND (@estadoId IS NULL OR p.estado_id = @estadoId)
                     AND (@date IS NULL OR DATE(p.fecha_hora) = @date);

                     SELECT p.*
                     FROM panic_alert p
                     INNER JOIN sesion_usuario s ON p.sesion_usuario_id = s.id
                     WHERE p.deleted_at IS NULL
                     AND s.deleted_at IS NULL
                     AND s.unidad_id = @unidadId
                     AND (@estadoId IS NULL OR p.estado_id = @estadoId)
                     AND (@date IS NULL OR DATE(p.fecha_hora) = @date)
                     ORDER BY p.fecha_hora DESC
                     LIMIT @pageSize OFFSET @offset;";

            var parametros = new DynamicParameters();
            parametros.Add("@unidadId", unidadId);
            parametros.Add("@estadoId", estadoId);
            parametros.Add("@date", date.HasValue ? date.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null);
            parametros.Add("@offset", offset);
            parametros.Add("@pageSize", pageSize);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                using var multi = await connection.QueryMultipleAsync(sql, parametros);
                var total = await multi.ReadSingleAsync<int>();
                var data = (await multi.ReadAsync<PanicAlert>()).AsList();

                return new PaginaDatos<PanicAlert>
                {
                    total = total,
                    page = page,
                    pageSize = pageSize,
                    totalPages = (int)Math.Ceiling(total / (double)pageSize),
                    data = data
                };
            }
        }

        public async Task<PaginaDatos<PanicAlert>> GetPagedByUsuarioIdAsync(Guid usuarioId, string? estadoId, int page = 1, int pageSize = 20, DateOnly? date = null)
        {
            var offset = (page - 1) * pageSize;
            var sql = @"
                     SELECT count(*)
                     FROM panic_alert p
                     INNER JOIN sesion_usuario s ON p.sesion_usuario_id = s.id
                     WHERE p.deleted_at IS NULL
                     AND s.deleted_at IS NULL
                     AND s.usuario_id = @usuarioId
                     AND (@estadoId IS NULL OR p.estado_id = @estadoId)
                     AND (@date IS NULL OR DATE(p.fecha_hora) = @date);

                     SELECT p.*
                     FROM panic_alert p
                     INNER JOIN sesion_usuario s ON p.sesion_usuario_id = s.id
                     WHERE p.deleted_at IS NULL
                     AND s.deleted_at IS NULL
                     AND s.usuario_id = @usuarioId
                     AND (@estadoId IS NULL OR p.estado_id = @estadoId)
                     AND (@date IS NULL OR DATE(p.fecha_hora) = @date)
                     ORDER BY p.fecha_hora DESC
                     LIMIT @pageSize OFFSET @offset;";

            var parametros = new DynamicParameters();
            parametros.Add("@usuarioId", usuarioId);
            parametros.Add("@estadoId", estadoId);
            parametros.Add("@date", date.HasValue ? date.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null);
            parametros.Add("@offset", offset);
            parametros.Add("@pageSize", pageSize);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                using var multi = await connection.QueryMultipleAsync(sql, parametros);
                var total = await multi.ReadSingleAsync<int>();
                var data = (await multi.ReadAsync<PanicAlert>()).AsList();

                return new PaginaDatos<PanicAlert>
                {
                    total = total,
                    page = page,
                    pageSize = pageSize,
                    totalPages = (int)Math.Ceiling(total / (double)pageSize),
                    data = data
                };
            }
        }

        public async Task UpdateEstadoAsync(Guid id, string estadoId, Guid? updatedBy = null)
        {
            const string sql = "UPDATE panic_alert SET estado_id = @estadoId, updated_at = now(), updated_by = @updatedBy WHERE id = @id AND deleted_at IS NULL";
            var p = new DynamicParameters();
            p.Add("@id", id);
            p.Add("@estadoId", estadoId);
            p.Add("@updatedBy", updatedBy);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                await connection.ExecuteAsync(sql, p);
            }
        }

        public async Task UpdateMensajeAsync(Guid id, string? mensaje, Guid? updatedBy = null)
        {
            const string sql = "UPDATE panic_alert SET mensaje = @mensaje, updated_at = now(), updated_by = @updatedBy WHERE id = @id AND deleted_at IS NULL";
            var p = new DynamicParameters();
            p.Add("@id", id);
            p.Add("@mensaje", mensaje);
            p.Add("@updatedBy", updatedBy);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                await connection.ExecuteAsync(sql, p);
            }
        }
    }
}
