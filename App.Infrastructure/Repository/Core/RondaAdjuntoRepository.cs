using App.Application.Interfaces.Core;
using App.Core.Entities;
using App.Core.Entities.Core;
using App.Infrastructure.Database;

namespace App.Infrastructure.Repository.Core
{
    public class RondaAdjuntoRepository : IRondaAdjuntoRepository
    {
        private readonly IDbConnectionFactory _dbFactory;

        public RondaAdjuntoRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<Guid> AddAsync(RondaAdjunto entity)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task AddOrUpdateAsync(RondaAdjunto entity)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(Guid id)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task<PaginaDatos<RondaAdjunto>> GetPagedAsync(string? search, int page = 1, int pageSize = 20)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task<RondaAdjunto?> GetByIdAsync(Guid id)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(RondaAdjunto entity)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }
    }
}
