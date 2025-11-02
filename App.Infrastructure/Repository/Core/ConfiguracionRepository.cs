using App.Application.Interfaces.Core;
using App.Core.Entities;
using App.Core.Entities.Core;
using Microsoft.Extensions.Configuration;

namespace App.Infrastructure.Repository.Core
{
    public class ConfiguracionRepository : IConfiguracionRepository
    {
        private readonly IConfiguration _config;

        public ConfiguracionRepository(IConfiguration config)
        {
            _config = config;
        }

        public Task<string> AddAsync(Configuracion entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<PaginaDatos<Configuracion>> FindAsync(string? search, int page = 1, int pageSize = 20)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Configuracion>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Configuracion?> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Configuracion entity)
        {
            throw new NotImplementedException();
        }
    }
}
