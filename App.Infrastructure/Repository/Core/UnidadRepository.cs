using App.Application.Interfaces.Core;
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
                        (cliente_id, unidad_id_padre, nombre, direccion, created_at, created_by)
                        VALUES
                        (@cliente_id, @unidad_id_padre, @nombre, @direccion, @created_at, @created_by)
                        RETURNING id";

            var p = new DynamicParameters();
            p.Add("@cliente_id", entity.cliente_id);
            p.Add("@unidad_id_padre", entity.unidad_id_padre);
            p.Add("@nombre", entity.nombre);
            p.Add("@direccion", entity.direccion);
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
                            updated_at = now(),
                            updated_by = @updated_by
                            WHERE id = @id";

            var p = new DynamicParameters();
            p.Add("@id", entity.id);
            p.Add("@cliente_id", entity.cliente_id);
            p.Add("@unidad_id_padre", entity.unidad_id_padre);
            p.Add("@nombre", entity.nombre);
            p.Add("@direccion", entity.direccion);
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

        public async Task<IReadOnlyList<Unidad>> GetAllByClienteIdAsync(Guid clienteId)
        {
            const string sql = "SELECT * FROM unidad WHERE cliente_id = @clienteId AND deleted_at IS NULL ORDER BY nombre";
            var p = new DynamicParameters();
            p.Add("@clienteId", clienteId);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                var items = await connection.QueryAsync<Unidad>(sql, p);
                return items.AsList();
            }
        }

        public async Task<bool> IsDescendantAsync(Guid unidadId, Guid possibleDescendantId)
        {
            const string sql = @"WITH RECURSIVE descendants AS (
 SELECT id, unidad_id_padre FROM unidad WHERE id = @unidadId
 UNION ALL
 SELECT u.id, u.unidad_id_padre FROM unidad u JOIN descendants d ON u.unidad_id_padre = d.id
 ) SELECT 1 FROM descendants WHERE id = @possibleId LIMIT 1";

            var p = new DynamicParameters();
            p.Add("@unidadId", unidadId);
            p.Add("@possibleId", possibleDescendantId);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                var found = await connection.QueryFirstOrDefaultAsync<int?>(sql, p);
                return found.HasValue && found.Value == 1;
            }
        }
    }
}
