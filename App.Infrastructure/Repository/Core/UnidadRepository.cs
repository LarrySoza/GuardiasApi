using App.Application.Interfaces.Core;
using App.Core.Entities;
using App.Core.Entities.Core;
using Microsoft.Extensions.Configuration;

namespace App.Infrastructure.Repository.Core
{
    public class UnidadRepository : IUnidadRepository
    {
        private readonly IConfiguration _config;

        public UnidadRepository(IConfiguration config)
        {
            _config = config;
        }

        public Task<Guid> AddAsync(Unidad entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<PaginaDatos<Unidad>> FindAsync(string? search, int page = 1, int pageSize = 20)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Unidad>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Unidad?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Unidad entity)
        {
            throw new NotImplementedException();
        }
    }
}
