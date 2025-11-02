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

        public Task<Guid> AddAsync(Cliente entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<PaginaDatos<Cliente>> FindAsync(string? search, int page = 1, int pageSize = 20)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Cliente>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Cliente?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Cliente entity)
        {
            throw new NotImplementedException();
        }
    }
}
