using App.Application.Interfaces.Core;
using App.Core.Entities;
using App.Core.Entities.Core;
using Microsoft.Extensions.Configuration;

namespace App.Infrastructure.Repository.Core
{
    public class RondaAdjuntoRepository : IRondaAdjuntoRepository
    {
        private readonly IConfiguration _config;

        public RondaAdjuntoRepository(IConfiguration configuration)
        {
            _config = configuration;
        }

        public Task<Guid> AddAsync(RondaAdjunto entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<PaginaDatos<RondaAdjunto>> FindAsync(string? search, int page = 1, int pageSize = 20)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<RondaAdjunto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<RondaAdjunto?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(RondaAdjunto entity)
        {
            throw new NotImplementedException();
        }
    }
}
