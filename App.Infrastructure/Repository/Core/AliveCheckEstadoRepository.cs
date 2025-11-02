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

        public async Task<PaginaDatos<AliveCheckEstado>> FindAsync(string? search, int page = 1, int pageSize = 20)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<AliveCheckEstado>> GetAllAsync()
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task<AliveCheckEstado?> GetByIdAsync(string id)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }
    }
}
