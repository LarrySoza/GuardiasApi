using App.Application.Interfaces.Core;
using App.Core.Entities;
using App.Core.Entities.Core;
using Microsoft.Extensions.Configuration;

namespace App.Infrastructure.Repository.Core
{
    public class AsignacionRepository : IAsignacionRepository
    {
        private readonly IConfiguration _config;

        public AsignacionRepository(IConfiguration configuration)
        {
            _config = configuration;
        }

        public Task<Guid> AddAsync(Asignacion entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<PaginaDatos<Asignacion>> FindAsync(string? search, int page = 1, int pageSize = 20)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Asignacion>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Asignacion?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Asignacion entity)
        {
            throw new NotImplementedException();
        }
    }
}
