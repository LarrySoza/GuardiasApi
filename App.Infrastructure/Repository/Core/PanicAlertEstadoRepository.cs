using App.Application.Interfaces.Core;
using App.Core.Entities;
using App.Core.Entities.Core;
using Microsoft.Extensions.Configuration;

namespace App.Infrastructure.Repository.Core
{
    public class PanicAlertEstadoRepository : IPanicAlertEstadoRepository
    {
        private readonly IConfiguration _config;

        public PanicAlertEstadoRepository(IConfiguration configuration)
        {
            _config = configuration;
        }

        public async Task<IReadOnlyList<PanicAlertEstado>> GetAllAsync()
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task<PanicAlertEstado?> GetByIdAsync(string id)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }
    }
}
