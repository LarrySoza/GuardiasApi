using App.Application.Interfaces.Core;
using App.Core.Entities;
using App.Core.Entities.Core;
using Microsoft.Extensions.Configuration;

namespace App.Infrastructure.Repository.Core
{
    public class IncidenteTipoRepository : IIncidenteTipoRepository
    {
        private readonly IConfiguration _config;

        public IncidenteTipoRepository(IConfiguration configuration)
        {
            _config = configuration;
        }

        public async Task<IReadOnlyList<IncidenteTipo>> GetAllAsync()
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task<IncidenteTipo?> GetByIdAsync(string id)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }
    }
}
