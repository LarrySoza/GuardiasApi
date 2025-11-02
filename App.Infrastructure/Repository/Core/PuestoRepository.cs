using App.Application.Interfaces.Core;
using App.Core.Entities;
using App.Core.Entities.Core;
using Microsoft.Extensions.Configuration;

namespace App.Infrastructure.Repository.Core
{
    public class PuestoRepository : IPuestoRepository
    {
        private readonly IConfiguration _config;

        public PuestoRepository(IConfiguration configuration)
        {
            _config = configuration;
        }

        public async Task<Guid> AddAsync(Puesto entity)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task AddOrUpdateAsync(Puesto entity)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(Guid id)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task<PaginaDatos<Puesto>> FindAsync(string? search, int page = 1, int pageSize = 20)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task<Puesto?> GetByIdAsync(Guid id)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(Puesto entity)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }
    }
}
