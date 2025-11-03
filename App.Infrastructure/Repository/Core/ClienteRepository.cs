using App.Application.Interfaces.Core;
using App.Core.Entities;
using App.Core.Entities.Core;
using App.Infrastructure.Database;
using Dapper;

namespace App.Infrastructure.Repository.Core
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly IDbConnectionFactory _dbFactory;

        public ClienteRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<Guid> AddAsync(Cliente entity)
        {
            const string sql = @"INSERT INTO cliente
                 (razon_social, ruc, created_at, created_by)
                 VALUES
                 (@razon_social, @ruc, @created_at, @created_by)
                 RETURNING id";

            var p = new DynamicParameters();
            p.Add("@razon_social", entity.razon_social);
            p.Add("@ruc", entity.ruc);
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

        public async Task AddOrUpdateAsync(Cliente entity)
        {
            // Si no tiene id o id es Guid.Empty -> Add, else Update
            if (entity.id == null || (entity.id is Guid g && g == Guid.Empty))
            {
                await AddAsync(entity);
                return;
            }

            await UpdateAsync(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            const string sql = "UPDATE cliente SET deleted_at = now() WHERE id = @id";
            var p = new DynamicParameters();
            p.Add("@id", id);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                await connection.ExecuteAsync(sql, p);
            }
        }

        public async Task<PaginaDatos<Cliente>> FindAsync(string? search, int page = 1, int pageSize = 20)
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
                 FROM cliente c
                 WHERE c.deleted_at IS NULL
                 AND (@search IS NULL
                 OR c.razon_social ILIKE '%' || @search || '%'
                 OR c.ruc ILIKE '%' || @search || '%');

                 SELECT c.*
                 FROM cliente c
                 WHERE c.deleted_at IS NULL
                 AND (@search IS NULL
                 OR c.razon_social ILIKE '%' || @search || '%'
                 OR c.ruc ILIKE '%' || @search || '%')
                 ORDER BY c.razon_social
                 LIMIT @pageSize OFFSET @offset;";

                using var multi = await connection.QueryMultipleAsync(sql, parametros);
                var total = await multi.ReadSingleAsync<int>();
                var data = (await multi.ReadAsync<Cliente>()).AsList();

                return new PaginaDatos<Cliente>
                {
                    total = total,
                    page = page,
                    pageSize = pageSize,
                    totalPages = (int)Math.Ceiling(total / (double)pageSize),
                    data = data
                };
            }
        }

        public async Task<Cliente?> GetByIdAsync(Guid id)
        {
            const string sql = "SELECT * FROM cliente WHERE id = @id AND deleted_at IS NULL";
            var p = new DynamicParameters();
            p.Add("@id", id);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                return await connection.QueryFirstOrDefaultAsync<Cliente>(sql, p);
            }
        }

        public async Task UpdateAsync(Cliente entity)
        {
            const string sql = @"UPDATE cliente SET
                 razon_social = @razon_social,
                 ruc = @ruc,
                 updated_at = now(),
                 updated_by = @updated_by
                 WHERE id = @id";

            var p = new DynamicParameters();
            p.Add("@id", entity.id);
            p.Add("@razon_social", entity.razon_social);
            p.Add("@ruc", entity.ruc);
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
