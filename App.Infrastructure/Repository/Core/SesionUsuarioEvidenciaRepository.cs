using App.Application.Interfaces.Core;
using App.Core.Entities;
using App.Core.Entities.Core;
using Microsoft.Extensions.Configuration;

namespace App.Infrastructure.Repository.Core
{
    public class SesionUsuarioEvidenciaRepository : ISesionUsuarioEvidenciaRepository
    {
        private readonly IConfiguration _config;

        public SesionUsuarioEvidenciaRepository(IConfiguration configuration)
        {
            _config = configuration;
        }

        public Task<Guid> AddAsync(SesionUsuarioEvidencia entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<PaginaDatos<SesionUsuarioEvidencia>> FindAsync(string? search, int page = 1, int pageSize = 20)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<SesionUsuarioEvidencia>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<SesionUsuarioEvidencia?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(SesionUsuarioEvidencia entity)
        {
            throw new NotImplementedException();
        }
    }
}
