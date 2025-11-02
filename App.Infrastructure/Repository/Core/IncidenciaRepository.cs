using App.Application.Interfaces.Core;
using App.Core.Entities;
using App.Core.Entities.Core;
using Microsoft.Extensions.Configuration;

namespace App.Infrastructure.Repository.Core
{
    public class IncidenciaRepository : IIncidenciaRepository
    {
        private readonly IConfiguration _config;

        public IncidenciaRepository(IConfiguration configuration)
        {
            _config = configuration;
        }

        public Task<Guid> AddAsync(Incidencia entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<PaginaDatos<Incidencia>> FindAsync(string? search, int page = 1, int pageSize = 20)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Incidencia>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Incidencia?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Incidencia entity)
        {
            throw new NotImplementedException();
        }
    }
}
