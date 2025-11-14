using App.Application.Interfaces.Core;
using App.Application.Models.Puesto;
using App.Application.Models.Turno;
using App.Core.Entities;
using App.Core.Entities.Core;
using App.Infrastructure.Database;
using Dapper;

namespace App.Infrastructure.Repository.Core
{
    public class PuestoRepository : IPuestoRepository
    {
        private readonly IDbConnectionFactory _dbFactory;
        private readonly IUsuarioUnidadRepository _usuarioUnidadRepository;

        public PuestoRepository(IDbConnectionFactory dbFactory, IUsuarioUnidadRepository usuarioUnidadRepository)
        {
            _dbFactory = dbFactory;
            _usuarioUnidadRepository = usuarioUnidadRepository;
        }

        public async Task<Guid> AddAsync(Puesto entity, List<int> turnosId)
        {
            if (turnosId == null || turnosId.Count == 0)
                throw new ArgumentException("Se requiere al menos un turno asociado al puesto.", nameof(turnosId));

            const string insertSql = @"INSERT INTO puesto
                                    (unidad_id, nombre, lat, lng, created_at, created_by)
                                    VALUES (@unidad_id, @nombre, @lat, @lng, @created_at, @created_by)
                                    RETURNING id";

            var p = new DynamicParameters();
            p.Add("@unidad_id", entity.unidad_id);
            p.Add("@nombre", entity.nombre);
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
                    var id = await connection.ExecuteScalarAsync<Guid>(insertSql, p, tx);

                    // Insertar turnos relacionados en tabla puente puesto_turno
                    const string insertRel = @"INSERT INTO puesto_turno (puesto_id, turno_id) VALUES (@puesto_id, @turno_id) ON CONFLICT (puesto_id, turno_id) DO NOTHING";
                    foreach (var tId in turnosId)
                    {
                        var pr = new DynamicParameters();
                        pr.Add("@puesto_id", id);
                        pr.Add("@turno_id", tId);
                        await connection.ExecuteAsync(insertRel, pr, tx);
                    }

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
            const string sql = "UPDATE puesto SET deleted_at = now() WHERE id = @id";
            var p = new DynamicParameters();
            p.Add("@id", id);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                await connection.ExecuteAsync(sql, p);
            }
        }

        public async Task UpdateAsync(Puesto entity, List<int> turnosId)
        {
            if (turnosId == null || turnosId.Count == 0)
                throw new ArgumentException("Se requiere al menos un turno asociado al puesto.", nameof(turnosId));

            const string sql = @"UPDATE puesto SET
                 nombre = @nombre,
                 lat = @lat,
                 lng = @lng,
                 updated_at = now(),
                 updated_by = @updated_by
                 WHERE id = @id";

            var p = new DynamicParameters();
            p.Add("@id", entity.id);
            p.Add("@nombre", entity.nombre);
            p.Add("@lat", entity.lat);
            p.Add("@lng", entity.lng);
            p.Add("@updated_by", entity.updated_by);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                using var tx = connection.BeginTransaction();
                try
                {
                    // Actualizar fila del puesto
                    await connection.ExecuteAsync(sql, p, tx);

                    // Eliminar relaciones antiguas en la tabla puente (borrado físico para asegurar re-inserción)
                    const string deleteRelSql = @"DELETE FROM puesto_turno WHERE puesto_id = @id";
                    var delParams = new DynamicParameters();
                    delParams.Add("@id", entity.id);
                    await connection.ExecuteAsync(deleteRelSql, delParams, tx);

                    // Insertar nuevas relaciones
                    const string insertRel = @"INSERT INTO puesto_turno (puesto_id, turno_id) VALUES (@puesto_id, @turno_id) ON CONFLICT (puesto_id, turno_id) DO NOTHING";
                    foreach (var tId in turnosId)
                    {
                        var pr = new DynamicParameters();
                        pr.Add("@puesto_id", entity.id);
                        pr.Add("@turno_id", tId);
                        await connection.ExecuteAsync(insertRel, pr, tx);
                    }

                    tx.Commit();
                }
                catch
                {
                    try { tx.Rollback(); } catch { }
                    throw;
                }
            }
        }

        // Nueva implementación: paginado que devuelve PuestoConTurnosDto
        public async Task<PaginaDatos<PuestoConTurnosDto>> GetPagedWithTurnosAsync(string? search, int page = 1, int pageSize = 20)
        {
            var offset = (page - 1) * pageSize;
            var parametros = new
            {
                search = string.IsNullOrWhiteSpace(search) ? null : search,
                offset,
                pageSize
            };

            const string countSql = @"SELECT count(*)
                                      FROM puesto p
                                      WHERE p.deleted_at IS NULL
                                      AND (@search IS NULL OR p.nombre ILIKE '%' || @search || '%');";

            const string dataSql = @"SELECT p.id, p.unidad_id, p.nombre, p.lat, p.lng, t.id as turno_id, t.nombre as turno_nombre
                                      FROM puesto p
                                      LEFT JOIN puesto_turno pt ON pt.puesto_id = p.id
                                      LEFT JOIN turno t ON t.id = pt.turno_id
                                      WHERE p.deleted_at IS NULL
                                      AND (@search IS NULL OR p.nombre ILIKE '%' || @search || '%')
                                      ORDER BY p.nombre
                                      LIMIT @pageSize OFFSET @offset;";

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();

                var total = await connection.QuerySingleAsync<int>(countSql, parametros);

                var lookup = new Dictionary<Guid, PuestoConTurnosDto>();

                var rows = await connection.QueryAsync(dataSql, parametros);
                foreach (var r in rows)
                {
                    Guid pid = r.id;
                    if (!lookup.TryGetValue(pid, out var dto))
                    {
                        dto = new PuestoConTurnosDto
                        {
                            id = r.id,
                            unidad_id = r.unidad_id,
                            nombre = r.nombre,
                            lat = r.lat,
                            lng = r.lng
                        };
                        lookup[pid] = dto;
                    }

                    if (r.turno_id != null)
                    {
                        dto.Turnos.Add(new TurnoDto
                        {
                            id = (int)r.turno_id,
                            nombre = r.turno_nombre
                        });
                    }
                }

                var lista = lookup.Values.ToList();

                return new PaginaDatos<PuestoConTurnosDto>
                {
                    total = total,
                    page = page,
                    pageSize = pageSize,
                    totalPages = (int)Math.Ceiling(total / (double)pageSize),
                    data = lista
                };
            }
        }

        public async Task<PuestoConTurnosDto?> GetByIdAsync(Guid id)
        {
            const string sql = @"SELECT p.id, p.unidad_id, p.nombre, p.lat, p.lng, t.id as turno_id, t.nombre as turno_nombre
                                 FROM puesto p
                                 LEFT JOIN puesto_turno pt ON pt.puesto_id = p.id
                                 LEFT JOIN turno t ON t.id = pt.turno_id
                                 WHERE p.id = @id AND p.deleted_at IS NULL;";

            var p = new DynamicParameters();
            p.Add("@id", id);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                var rows = await connection.QueryAsync(sql, p);

                if (!rows.Any()) return null;

                PuestoConTurnosDto? result = null;

                foreach (var r in rows)
                {
                    if (result == null)
                    {
                        result = new PuestoConTurnosDto
                        {
                            id = r.id,
                            unidad_id = r.unidad_id,
                            nombre = r.nombre,
                            lat = r.lat,
                            lng = r.lng
                        };
                    }

                    if (r.turno_id != null)
                    {
                        result.Turnos.Add(new TurnoDto
                        {
                            id = (int)r.turno_id,
                            nombre = r.turno_nombre
                        });
                    }
                }

                return result;
            }
        }

        public async Task<IReadOnlyList<PuestoConTurnosDto>> GetAllByUnidadIdAsync(Guid unidadId)
        {
            const string sql = @"SELECT p.id, p.unidad_id, p.nombre, p.lat, p.lng, t.id as turno_id, t.nombre as turno_nombre
                                 FROM puesto p
                                 LEFT JOIN puesto_turno pt ON pt.puesto_id = p.id
                                 LEFT JOIN turno t ON t.id = pt.turno_id
                                 WHERE p.unidad_id = @unidadId AND p.deleted_at IS NULL
                                 ORDER BY p.nombre;";

            var parametros = new DynamicParameters();
            parametros.Add("@unidadId", unidadId);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();

                var rows = await connection.QueryAsync(sql, parametros);

                var lookup = new Dictionary<Guid, PuestoConTurnosDto>();

                foreach (var r in rows)
                {
                    Guid pid = r.id;
                    if (!lookup.TryGetValue(pid, out var dto))
                    {
                        dto = new PuestoConTurnosDto
                        {
                            id = r.id,
                            unidad_id = r.unidad_id,
                            nombre = r.nombre,
                            lat = r.lat,
                            lng = r.lng
                        };
                        lookup[pid] = dto;
                    }

                    if (r.turno_id != null)
                    {
                        lookup[pid].Turnos.Add(new TurnoDto
                        {
                            id = (int)r.turno_id,
                            nombre = r.turno_nombre
                        });
                    }
                }

                return lookup.Values.ToList();
            }
        }

        public async Task<IReadOnlyList<PuestoConTurnosDto>> GetAllByUsuarioIdAsync(Guid userId)
        {
            // Obtener las unidades directamente asignadas al usuario
            var assignedUnidades = (await _usuarioUnidadRepository.GetAllAsync(userId)).Select(u => u.id).ToArray();

            if (assignedUnidades == null || assignedUnidades.Length == 0)
                return new List<PuestoConTurnosDto>();

            const string sql = @"WITH RECURSIVE descendants AS (
                                    SELECT id
                                    FROM unidad
                                    WHERE id = ANY(@assigned)
                                  UNION ALL
                                    SELECT u.id
                                    FROM unidad u
                                    JOIN descendants d ON u.unidad_id_padre = d.id
                                 )
                                 SELECT p.id, p.unidad_id, p.nombre, p.lat, p.lng, t.id as turno_id, t.nombre as turno_nombre
                                 FROM puesto p
                                 LEFT JOIN puesto_turno pt ON pt.puesto_id = p.id
                                 LEFT JOIN turno t ON t.id = pt.turno_id
                                 WHERE p.unidad_id IN (SELECT id FROM descendants)
                                 AND p.deleted_at IS NULL
                                 ORDER BY p.nombre;";

            var parametros = new DynamicParameters();
            parametros.Add("@assigned", assignedUnidades);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                var rows = await connection.QueryAsync(sql, parametros);

                var lookup = new Dictionary<Guid, PuestoConTurnosDto>();

                foreach (var r in rows)
                {
                    Guid pid = r.id;
                    if (!lookup.TryGetValue(pid, out var dto))
                    {
                        dto = new PuestoConTurnosDto
                        {
                            id = r.id,
                            unidad_id = r.unidad_id,
                            nombre = r.nombre,
                            lat = r.lat,
                            lng = r.lng
                        };
                        lookup[pid] = dto;
                    }

                    if (r.turno_id != null)
                    {
                        lookup[pid].Turnos.Add(new TurnoDto
                        {
                            id = (int)r.turno_id,
                            nombre = r.turno_nombre
                        });
                    }
                }

                return lookup.Values.ToList();
            }
        }
    }
}
