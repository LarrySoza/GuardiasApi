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

        public async Task<PaginaDatos<AsignacionEventoTipo>> FindAsync(string? search, int page = 1, int pageSize = 20)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<AsignacionEventoTipo>> GetAllAsync()
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task<AsignacionEventoTipo?> GetByIdAsync(string id)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }
    }
}
