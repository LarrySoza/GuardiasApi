using App.Application.Interfaces.Core;
using App.Core.Entities;
using App.Core.Entities.Core;
using Microsoft.Extensions.Configuration;

namespace App.Infrastructure.Repository.Core
{
    public class AliveCheckEstadoRepository : IAliveCheckEstadoRepository
    {
        private readonly IConfiguration _config;

        public AliveCheckEstadoRepository(IConfiguration config)
        {
            _config = config;
        }

        public Task<PaginaDatos<AliveCheckEstado>> FindAsync(string? search, int page = 1, int pageSize = 20)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<AliveCheckEstado>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<AliveCheckEstado?> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
