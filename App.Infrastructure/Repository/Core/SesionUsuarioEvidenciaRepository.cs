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

        public async Task<Guid> AddAsync(SesionUsuarioEvidencia entity)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task AddOrUpdateAsync(SesionUsuarioEvidencia entity)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(Guid id)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task<PaginaDatos<SesionUsuarioEvidencia>> FindAsync(string? search, int page = 1, int pageSize = 20)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task<SesionUsuarioEvidencia?> GetByIdAsync(Guid id)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(SesionUsuarioEvidencia entity)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }
    }
}
