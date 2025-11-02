using App.Application.Interfaces.Core;
using App.Core.Entities;
using App.Core.Entities.Core;
using Microsoft.Extensions.Configuration;

namespace App.Infrastructure.Repository.Core
{
    public class SesionUsuarioRepository : ISesionUsuarioRepository
    {
        private readonly IConfiguration _config;

        public SesionUsuarioRepository(IConfiguration configuration)
        {
            _config = configuration;
        }

        public Task<Guid> AddAsync(SesionUsuario entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<PaginaDatos<SesionUsuario>> FindAsync(string? search, int page = 1, int pageSize = 20)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<SesionUsuario>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<SesionUsuario?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(SesionUsuario entity)
        {
            throw new NotImplementedException();
        }
    }
}
