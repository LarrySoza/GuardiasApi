using App.Application.Interfaces.Core;
using App.Core.Entities;
using App.Core.Entities.Core;
using Microsoft.Extensions.Configuration;

namespace App.Infrastructure.Repository.Core
{
    public class AliveCheckRepository : IAliveCheckRepository
    {
        private readonly IConfiguration _config;

        public AliveCheckRepository(IConfiguration configuration)
        {
            _config = configuration;
        }

        public Task<Guid> AddAsync(AliveCheck entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<PaginaDatos<AliveCheck>> FindAsync(string? search, int page = 1, int pageSize = 20)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<AliveCheck>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<AliveCheck?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(AliveCheck entity)
        {
            throw new NotImplementedException();
        }
    }
}
