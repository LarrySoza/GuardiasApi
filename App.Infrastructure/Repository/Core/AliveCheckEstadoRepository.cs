using App.Application.Interfaces.Core;
using App.Core.Entities;
using App.Core.Entities.Core;
using Microsoft.Extensions.Configuration;

namespace App.Infrastructure.Repository.Core
{
    public class AliveCheckEstadoRepository : IAliveCheckEstadoRepository
    {
        private readonly IConfiguration _config;

        public AliveCheckEstadoRepository(IConfiguration config)
        {
            _config = config;
        }

        public Task<IReadOnlyList<AliveCheckEstado>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<AliveCheckEstado?> GetByIdAsync(string id)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }
    }
}
