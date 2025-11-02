using App.Application.Interfaces.Core;
using App.Core.Entities;
using App.Core.Entities.Core;
using Microsoft.Extensions.Configuration;

namespace App.Infrastructure.Repository.Core
{
    public class AsignacionEventoRepository : IAsignacionEventoRepository
    {
        private readonly IConfiguration _config;

        public AsignacionEventoRepository(IConfiguration configuration)
        {
            _config = configuration;
        }

        public Task<Guid> AddAsync(AsignacionEvento entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<PaginaDatos<AsignacionEvento>> FindAsync(string? search, int page = 1, int pageSize = 20)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<AsignacionEvento>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<AsignacionEvento?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(AsignacionEvento entity)
        {
            throw new NotImplementedException();
        }
    }
}
