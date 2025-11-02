using App.Application.Interfaces.Core;
using App.Core.Entities;
using App.Core.Entities.Core;
using Microsoft.Extensions.Configuration;

namespace App.Infrastructure.Repository.Core
{
    public class IncidenteTipoRepository : IIncidenteTipoRepository
    {
        private readonly IConfiguration _config;

        public IncidenteTipoRepository(IConfiguration configuration)
        {
            _config = configuration;
        }

        public Task<Guid> AddAsync(IncidenteTipo entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<PaginaDatos<IncidenteTipo>> FindAsync(string? search, int page = 1, int pageSize = 20)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<IncidenteTipo>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IncidenteTipo?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(IncidenteTipo entity)
        {
            throw new NotImplementedException();
        }
    }
}
