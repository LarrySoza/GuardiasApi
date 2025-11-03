using App.Application.Interfaces.Core;
using App.Core.Entities;
using App.Core.Entities.Core;
using App.Infrastructure.Database;

namespace App.Infrastructure.Repository.Core
{
    public class PanicAlertAdjuntoRepository : IPanicAlertAdjuntoRepository
    {
        private readonly IDbConnectionFactory _dbFactory;

        public PanicAlertAdjuntoRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<Guid> AddAsync(PanicAlertAdjunto entity)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task AddOrUpdateAsync(PanicAlertAdjunto entity)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(Guid id)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task<PaginaDatos<PanicAlertAdjunto>> GetPagedAsync(string? search, int page = 1, int pageSize = 20)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task<PanicAlertAdjunto?> GetByIdAsync(Guid id)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(PanicAlertAdjunto entity)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }
    }
}
