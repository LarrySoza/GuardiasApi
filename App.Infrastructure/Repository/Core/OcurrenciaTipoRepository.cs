using App.Application.Interfaces.Core;
using App.Core.Entities;
using App.Core.Entities.Core;
using Microsoft.Extensions.Configuration;

namespace App.Infrastructure.Repository.Core
{
    public class OcurrenciaTipoRepository : IOcurrenciaTipoRepository
    {
        private readonly IConfiguration _config;

        public OcurrenciaTipoRepository(IConfiguration configuration)
        {
            _config = configuration;
        }

        public async Task<IReadOnlyList<OcurrenciaTipo>> GetAllAsync()
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task<OcurrenciaTipo?> GetByIdAsync(string id)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }
    }
}
