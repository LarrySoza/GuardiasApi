using App.Application.Interfaces.Core;
using App.Core.Entities;
using App.Core.Entities.Core;
using Microsoft.Extensions.Configuration;

namespace App.Infrastructure.Repository.Core
{
    public class PanicAlertAdjuntoTipoRepository : IPanicAlertAdjuntoTipoRepository
    {
        private readonly IConfiguration _config;

        public PanicAlertAdjuntoTipoRepository(IConfiguration config)
        {
            _config = config;
        }

        public Task<PaginaDatos<PanicAlertAdjuntoTipo>> FindAsync(string? search, int page = 1, int pageSize = 20)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<PanicAlertAdjuntoTipo>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<PanicAlertAdjuntoTipo?> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
