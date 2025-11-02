using App.Application.Interfaces.Core;
using App.Core.Entities;
using App.Core.Entities.Core;
using Microsoft.Extensions.Configuration;

namespace App.Infrastructure.Repository.Core
{
    public class OcurrenciaAdjuntoRepository : IOcurrenciaAdjuntoRepository
    {
        private readonly IConfiguration _config;

        public OcurrenciaAdjuntoRepository(IConfiguration configuration)
        {
            _config = configuration;
        }

        public Task<Guid> AddAsync(OcurrenciaAdjunto entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<PaginaDatos<OcurrenciaAdjunto>> FindAsync(string? search, int page = 1, int pageSize = 20)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<OcurrenciaAdjunto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<OcurrenciaAdjunto?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(OcurrenciaAdjunto entity)
        {
            throw new NotImplementedException();
        }
    }
}
