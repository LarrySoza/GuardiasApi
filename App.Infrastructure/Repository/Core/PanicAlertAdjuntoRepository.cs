using App.Application.Interfaces.Core;
using App.Core.Entities;
using App.Core.Entities.Core;
using Microsoft.Extensions.Configuration;

namespace App.Infrastructure.Repository.Core
{
    public class PanicAlertAdjuntoRepository : IPanicAlertAdjuntoRepository
    {
        private readonly IConfiguration _config;

        public PanicAlertAdjuntoRepository(IConfiguration configuration)
        {
            _config = configuration;
        }

        public Task<Guid> AddAsync(PanicAlertAdjunto entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<PaginaDatos<PanicAlertAdjunto>> FindAsync(string? search, int page = 1, int pageSize = 20)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<PanicAlertAdjunto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<PanicAlertAdjunto?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(PanicAlertAdjunto entity)
        {
            throw new NotImplementedException();
        }
    }
}
