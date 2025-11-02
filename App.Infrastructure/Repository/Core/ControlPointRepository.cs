using App.Application.Interfaces.Core;
using App.Core.Entities;
using App.Core.Entities.Core;
using Microsoft.Extensions.Configuration;

namespace App.Infrastructure.Repository.Core
{
    public class ControlPointRepository : IControlPointRepository
    {
        private readonly IConfiguration _config;

        public ControlPointRepository(IConfiguration configuration)
        {
            _config = configuration;
        }

        public Task<Guid> AddAsync(ControlPoint entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<PaginaDatos<ControlPoint>> FindAsync(string? search, int page = 1, int pageSize = 20)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<ControlPoint>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ControlPoint?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(ControlPoint entity)
        {
            throw new NotImplementedException();
        }
    }
}
