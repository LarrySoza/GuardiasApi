using App.Application.Interfaces.Core;
using App.Core.Entities;
using App.Core.Entities.Core;
using Microsoft.Extensions.Configuration;

namespace App.Infrastructure.Repository.Core
{
    public class OcurrenciaRepository : IOcurrenciaRepository
    {
        private readonly IConfiguration _config;

        public OcurrenciaRepository(IConfiguration configuration)
        {
            _config = configuration;
        }

        public async Task<Guid> AddAsync(Ocurrencia entity)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task AddOrUpdateAsync(Ocurrencia entity)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(Guid id)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task<PaginaDatos<Ocurrencia>> FindAsync(string? search, int page = 1, int pageSize = 20)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task<Ocurrencia?> GetByIdAsync(Guid id)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(Ocurrencia entity)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }
    }
}
