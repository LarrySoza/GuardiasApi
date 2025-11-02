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

        public async Task<IReadOnlyList<TipoConfiguracion>> GetAllAsync()
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task<TipoConfiguracion?> GetByIdAsync(string id)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }
    }
}
