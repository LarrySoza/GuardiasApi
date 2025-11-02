using App.Application.Interfaces.Core;
using App.Core.Entities;
using App.Core.Entities.Core;
using Microsoft.Extensions.Configuration;

namespace App.Infrastructure.Repository.Core
{
    public class TipoConfiguracionRepository : ITipoConfiguracionRepository
    {
        private readonly IConfiguration _config;

        public TipoConfiguracionRepository(IConfiguration configuration)
        {
            _config = configuration;
        }

        public Task<PaginaDatos<TipoConfiguracion>> FindAsync(string? search, int page = 1, int pageSize = 20)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<TipoConfiguracion>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<TipoConfiguracion?> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
