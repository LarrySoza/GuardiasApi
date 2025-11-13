using App.Application.Interfaces;
using App.Application.Interfaces.Core;
using App.Core.Entities;
using App.Core.Entities.Core;
using App.Infrastructure.Database;
using Dapper;

namespace App.Infrastructure.Repository.Core
{
    public class SesionUsuarioRepository : ISesionUsuarioRepository
    {
        private readonly IDbConnectionFactory _dbFactory;
        private readonly IFileStorageService _fileStorage;

        public SesionUsuarioRepository(IDbConnectionFactory dbFactory, IFileStorageService fileStorage)
        {
            _dbFactory = dbFactory;
            _fileStorage = fileStorage;
        }

        public async Task<Guid> AddAsync(SesionUsuario entity, Stream content, string originalFileName, CancellationToken cancellationToken = default)
        {
            string? rutaFotoInicio = null;

            if (content != null)
            {
                rutaFotoInicio = await _fileStorage.SaveAsync(content, originalFileName, "SesionUsuario", cancellationToken);
            }

            const string insertSql = @"INSERT INTO sesion_usuario
                                       (usuario_id, cliente_id, unidad_id, puesto_id, turno_id, fecha_inicio, ruta_foto_inicio, created_at, created_by)
                                       VALUES (@usuario_id, @cliente_id, @unidad_id, @puesto_id, @turno_id, @fecha_inicio, @ruta_foto_inicio, @created_at, @created_by)
                                       RETURNING id";

            var p = new DynamicParameters();
            p.Add("@usuario_id", entity.usuario_id);
            p.Add("@cliente_id", entity.cliente_id);
            p.Add("@unidad_id", entity.unidad_id);
            p.Add("@puesto_id", entity.puesto_id);
            p.Add("@turno_id", entity.turno_id);
            p.Add("@fecha_inicio", entity.fecha_inicio == default ? DateTimeOffset.UtcNow : entity.fecha_inicio);
            p.Add("@ruta_foto_inicio", rutaFotoInicio);
            p.Add("@created_at", entity.created_at == null ? DateTimeOffset.UtcNow : entity.created_at);
            p.Add("@created_by", entity.created_by);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                using var tx = connection.BeginTransaction();
                try
                {
                    var sessionId = await connection.ExecuteScalarAsync<Guid>(insertSql, p, tx);
                    tx.Commit();
                    return sessionId;
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
            const string sql = "UPDATE sesion_usuario SET deleted_at = now() WHERE id = @id";
            var p = new DynamicParameters();
            p.Add("@id", id);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                await connection.ExecuteAsync(sql, p);
            }
        }

        public async Task<SesionUsuario?> GetByIdAsync(Guid id)
        {
            const string sql = "SELECT * FROM sesion_usuario WHERE id = @id AND deleted_at IS NULL";
            var p = new DynamicParameters();
            p.Add("@id", id);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                return await connection.QueryFirstOrDefaultAsync<SesionUsuario>(sql, p);
            }
        }

        public async Task<PaginaDatos<SesionUsuario>> GetPagedByClienteIdAsync(Guid clienteId, int page = 1, int pageSize = 20, DateOnly? date = null)
        {
            var offset = (page - 1) * pageSize;
            var sql = @"
                         SELECT count(*)
                         FROM sesion_usuario s
                         WHERE s.deleted_at IS NULL
                         AND s.cliente_id = @clienteId
                         AND (@date IS NULL OR DATE(s.fecha_inicio) = @date);

                         SELECT s.*
                         FROM sesion_usuario s
                         WHERE s.deleted_at IS NULL
                         AND s.cliente_id = @clienteId
                         AND (@date IS NULL OR DATE(s.fecha_inicio) = @date)
                         ORDER BY s.fecha_inicio DESC
                         LIMIT @pageSize OFFSET @offset;";

            var parametros = new DynamicParameters();
            parametros.Add("@clienteId", clienteId);
            parametros.Add("@date", date.HasValue ? date.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null);
            parametros.Add("@offset", offset);
            parametros.Add("@pageSize", pageSize);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                using var multi = await connection.QueryMultipleAsync(sql, parametros);
                var total = await multi.ReadSingleAsync<int>();
                var data = (await multi.ReadAsync<SesionUsuario>()).AsList();

                return new PaginaDatos<SesionUsuario>
                {
                    total = total,
                    page = page,
                    pageSize = pageSize,
                    totalPages = (int)Math.Ceiling(total / (double)pageSize),
                    data = data
                };
            }
        }

        public async Task<PaginaDatos<SesionUsuario>> GetPagedByUnidadIdAsync(Guid unidadId, int page = 1, int pageSize = 20, DateOnly? date = null)
        {
            var offset = (page - 1) * pageSize;
            var sql = @"
                         SELECT count(*)
                         FROM sesion_usuario s
                         WHERE s.deleted_at IS NULL
                         AND s.unidad_id = @unidadId
                         AND (@date IS NULL OR DATE(s.fecha_inicio) = @date);

                         SELECT s.*
                         FROM sesion_usuario s
                         WHERE s.deleted_at IS NULL
                         AND s.unidad_id = @unidadId
                         AND (@date IS NULL OR DATE(s.fecha_inicio) = @date)
                         ORDER BY s.fecha_inicio DESC
                         LIMIT @pageSize OFFSET @offset;";

            var parametros = new DynamicParameters();
            parametros.Add("@unidadId", unidadId);
            parametros.Add("@date", date.HasValue ? date.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null);
            parametros.Add("@offset", offset);
            parametros.Add("@pageSize", pageSize);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                using var multi = await connection.QueryMultipleAsync(sql, parametros);
                var total = await multi.ReadSingleAsync<int>();
                var data = (await multi.ReadAsync<SesionUsuario>()).AsList();

                return new PaginaDatos<SesionUsuario>
                {
                    total = total,
                    page = page,
                    pageSize = pageSize,
                    totalPages = (int)Math.Ceiling(total / (double)pageSize),
                    data = data
                };
            }
        }

        public async Task<PaginaDatos<SesionUsuario>> GetPagedByUsuarioIdAsync(Guid usuarioId, int page = 1, int pageSize = 20, DateOnly? date = null)
        {
            var offset = (page - 1) * pageSize;
            var sql = @"
 SELECT count(*)
 FROM sesion_usuario s
 WHERE s.deleted_at IS NULL
 AND s.usuario_id = @usuarioId
 AND (@date IS NULL OR DATE(s.fecha_inicio) = @date);

 SELECT s.*
 FROM sesion_usuario s
 WHERE s.deleted_at IS NULL
 AND s.usuario_id = @usuarioId
 AND (@date IS NULL OR DATE(s.fecha_inicio) = @date)
 ORDER BY s.fecha_inicio DESC
 LIMIT @pageSize OFFSET @offset;";

            var parametros = new DynamicParameters();
            parametros.Add("@usuarioId", usuarioId);
            parametros.Add("@date", date.HasValue ? date.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null);
            parametros.Add("@offset", offset);
            parametros.Add("@pageSize", pageSize);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                using var multi = await connection.QueryMultipleAsync(sql, parametros);
                var total = await multi.ReadSingleAsync<int>();
                var data = (await multi.ReadAsync<SesionUsuario>()).AsList();

                return new PaginaDatos<SesionUsuario>
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
