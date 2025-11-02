using App.Application.Interfaces.Core;
using App.Core.Entities;
using App.Core.Entities.Core;
using Microsoft.Extensions.Configuration;

namespace App.Infrastructure.Repository.Core
{
    public class AsignacionEventoTipoRepository : IAsignacionEventoTipoRepository
    {
        private readonly IConfiguration _config;

        public AsignacionEventoTipoRepository(IConfiguration configuration)
        {
            _config = configuration;
        }

        public Task<PaginaDatos<AsignacionEventoTipo>> FindAsync(string? search, int page = 1, int pageSize = 20)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<AsignacionEventoTipo>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<AsignacionEventoTipo?> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
