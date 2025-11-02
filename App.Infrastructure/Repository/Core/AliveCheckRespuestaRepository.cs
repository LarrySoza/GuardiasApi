using App.Application.Interfaces.Core;
using App.Core.Entities;
using App.Core.Entities.Core;
using Microsoft.Extensions.Configuration;

namespace App.Infrastructure.Repository.Core
{
    public class AliveCheckRespuestaRepository : IAliveCheckRespuestaRepository
    {
        private readonly IConfiguration _config;

        public AliveCheckRespuestaRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<Guid> AddAsync(AliveCheckRespuesta entity)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(Guid id)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task<PaginaDatos<AliveCheckRespuesta>> FindAsync(string? search, int page = 1, int pageSize = 20)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<AliveCheckRespuesta>> GetAllAsync()
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task<AliveCheckRespuesta?> GetByIdAsync(Guid id)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(AliveCheckRespuesta entity)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }
    }
}
