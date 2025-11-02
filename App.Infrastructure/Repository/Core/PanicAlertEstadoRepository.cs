using App.Application.Interfaces.Core;
using App.Core.Entities;
using App.Core.Entities.Core;
using Microsoft.Extensions.Configuration;

namespace App.Infrastructure.Repository.Core
{
    public class PanicAlertEstadoRepository : IPanicAlertEstadoRepository
    {
        private readonly IConfiguration _config;

        public PanicAlertEstadoRepository(IConfiguration configuration)
        {
            _config = configuration;
        }

        public Task<PaginaDatos<PanicAlertEstado>> FindAsync(string? search, int page = 1, int pageSize = 20)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<PanicAlertEstado>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<PanicAlertEstado?> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
