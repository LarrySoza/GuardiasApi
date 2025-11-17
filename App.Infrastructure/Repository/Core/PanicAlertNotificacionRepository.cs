using App.Application.Interfaces.Core;
using App.Application.Models.PanicAlert;
using App.Core.Entities;
using App.Infrastructure.Database;
using Dapper;

namespace App.Infrastructure.Repository.Core
{
    public class PanicAlertNotificacionRepository : IPanicAlertNotificacionRepository
    {
        private readonly IDbConnectionFactory _dbFactory;

        public PanicAlertNotificacionRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<List<Guid>> AddAsync(Guid panicAlertId, IReadOnlyList<Guid> usuarios)
        {
            if (usuarios == null || usuarios.Count == 0)
                throw new ArgumentException("La lista de usuarios no puede estar vacía", nameof(usuarios));

            const string insertSql = @"INSERT INTO panic_alert_notificacion
                                        (panic_alert_id, usuario_notificado_id, fecha_hora, created_at)
                                       VALUES (@panicAlertId, @usuarioId, now(), now())
                                       ON CONFLICT (panic_alert_id, usuario_notificado_id)
                                       DO UPDATE SET deleted_at = NULL, updated_at = now()
                                       RETURNING id";

            var affectedIds = new List<Guid>();

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                using var tx = connection.BeginTransaction();
                try
                {
                    foreach (var u in usuarios.Distinct())
                    {
                        var id = await connection.ExecuteScalarAsync<Guid?>(insertSql, new { panicAlertId, usuarioId = u }, tx);
                        if (id.HasValue)
                            affectedIds.Add(id.Value);
                    }

                    tx.Commit();
                }
                catch
                {
                    try { tx.Rollback(); } catch { }
                    throw;
                }

                return affectedIds;
            }
        }

        public async Task Confirm(Guid panicAlertNotificacionId, Guid usuarioId)
        {
            const string sql = "UPDATE panic_alert_notificacion SET aceptada = true, updated_at = now(), updated_by = @updatedBy WHERE id = @id AND deleted_at IS NULL";
            var p = new DynamicParameters();
            p.Add("@id", panicAlertNotificacionId);
            p.Add("@updatedBy", usuarioId);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                await connection.ExecuteAsync(sql, p);
            }
        }

        public async Task<PanicAlertNotificacionDto?> GetByIdAsync(Guid id)
        {
            const string sql = @"SELECT n.id,
                                     n.panic_alert_id,
                                     n.usuario_notificado_id,
                                     n.fecha_hora,
                                     n.aceptada,
                                     n.created_at,
                                     n.created_by,
                                     n.updated_at,
                                     n.updated_by,
                                     n.deleted_at,
                                     u.nombre_usuario,
                                     u.tipo_documento_id AS tipo_documento,
                                     u.numero_documento,
                                     u.nombre_completo
                                 FROM panic_alert_notificacion n
                                 LEFT JOIN usuario u ON n.usuario_notificado_id = u.id
                                 WHERE n.id = @id
                                 AND n.deleted_at IS NULL;";

            var p = new DynamicParameters();
            p.Add("@id", id);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                return await connection.QueryFirstOrDefaultAsync<PanicAlertNotificacionDto>(sql, p);
            }
        }

        public async Task<PaginaDatos<PanicAlertNotificacionDto>> GetPagedAsync(Guid panicAlertId, int page = 1, int pageSize = 20)
        {
            var offset = (page - 1) * pageSize;
            var parametros = new DynamicParameters();
            parametros.Add("@panicAlertId", panicAlertId);
            parametros.Add("@offset", offset);
            parametros.Add("@pageSize", pageSize);

            var sql = @"
                 SELECT count(*)
                 FROM panic_alert_notificacion n
                 WHERE n.deleted_at IS NULL
                 AND n.panic_alert_id = @panicAlertId;

                 SELECT n.id,
                        n.panic_alert_id,
                        n.usuario_notificado_id,
                        n.fecha_hora,
                        n.aceptada,
                        n.created_at,
                        n.created_by,
                        n.updated_at,
                        n.updated_by,
                        n.deleted_at,
                        u.nombre_usuario,
                        u.tipo_documento_id AS tipo_documento,
                        u.numero_documento,
                        u.nombre_completo
                 FROM panic_alert_notificacion n
                 LEFT JOIN usuario u ON n.usuario_notificado_id = u.id
                 WHERE n.deleted_at IS NULL
                 AND n.panic_alert_id = @panicAlertId
                 ORDER BY n.fecha_hora DESC
                 LIMIT @pageSize OFFSET @offset;";

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                using var multi = await connection.QueryMultipleAsync(sql, parametros);
                var total = await multi.ReadSingleAsync<int>();
                var data = (await multi.ReadAsync<PanicAlertNotificacionDto>()).AsList();

                return new PaginaDatos<PanicAlertNotificacionDto>
                {
                    total = total,
                    page = page,
                    pageSize = pageSize,
                    totalPages = (int)Math.Ceiling(total / (double)pageSize),
                    data = data
                };
            }
        }
    }
}
