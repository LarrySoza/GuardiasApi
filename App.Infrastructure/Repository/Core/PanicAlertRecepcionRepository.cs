using App.Application.Interfaces.Core;
using App.Core.Entities;
using App.Core.Entities.Core;
using Microsoft.Extensions.Configuration;

namespace App.Infrastructure.Repository.Core
{
    public class PanicAlertRecepcionRepository : IPanicAlertRecepcionRepository
    {
        private readonly IConfiguration _config;

        public PanicAlertRecepcionRepository(IConfiguration configuration)
        {
            _config = configuration;
        }

        public Task<Guid> AddAsync(PanicAlertRecepcion entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<PaginaDatos<PanicAlertRecepcion>> FindAsync(string? search, int page = 1, int pageSize = 20)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<PanicAlertRecepcion>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<PanicAlertRecepcion?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(PanicAlertRecepcion entity)
        {
            throw new NotImplementedException();
        }
    }
}
