using App.Application.Interfaces.Core;
using App.Core.Entities;
using App.Core.Entities.Core;
using App.Infrastructure.Database;

namespace App.Infrastructure.Repository.Core
{
    public class AliveCheckRespuestaRepository : IAliveCheckRespuestaRepository
    {
        private readonly IDbConnectionFactory _dbFactory;

        public AliveCheckRespuestaRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<Guid> AddAsync(AliveCheckRespuesta entity)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task AddOrUpdateAsync(AliveCheckRespuesta entity)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(Guid id)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task<PaginaDatos<AliveCheckRespuesta>> GetPagedAsync(string? search, int page = 1, int pageSize = 20)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task<AliveCheckRespuesta?> GetByIdAsync(Guid id)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(AliveCheckRespuesta entity)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }
    }
}
