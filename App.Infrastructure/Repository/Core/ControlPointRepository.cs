using App.Application.Interfaces.Core;
using App.Core.Entities;
using App.Core.Entities.Core;
using App.Infrastructure.Database;

namespace App.Infrastructure.Repository.Core
{
    public class ControlPointRepository : IControlPointRepository
    {
        private readonly IDbConnectionFactory _dbFactory;

        public ControlPointRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<Guid> AddAsync(ControlPoint entity)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task AddOrUpdateAsync(ControlPoint entity)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(Guid id)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task<PaginaDatos<ControlPoint>> GetPagedAsync(string? search, int page = 1, int pageSize = 20)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task<ControlPoint?> GetByIdAsync(Guid id)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(ControlPoint entity)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }
    }
}
