using App.Application.Interfaces.Core;
using App.Core.Entities;
using App.Core.Entities.Core;
using Microsoft.Extensions.Configuration;

namespace App.Infrastructure.Repository.Core
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly IConfiguration _config;

        public ClienteRepository(IConfiguration configuration)
        {
            _config = configuration;
        }

        public async Task<Guid> AddAsync(Cliente entity)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task AddOrUpdateAsync(Cliente entity)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(Guid id)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task<PaginaDatos<Cliente>> FindAsync(string? search, int page = 1, int pageSize = 20)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task<Cliente?> GetByIdAsync(Guid id)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(Cliente entity)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }
    }
}
