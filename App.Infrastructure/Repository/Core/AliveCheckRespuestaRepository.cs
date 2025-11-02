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

        public Task<Guid> AddAsync(AliveCheckRespuesta entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<PaginaDatos<AliveCheckRespuesta>> FindAsync(string? search, int page = 1, int pageSize = 20)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<AliveCheckRespuesta>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<AliveCheckRespuesta?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(AliveCheckRespuesta entity)
        {
            throw new NotImplementedException();
        }
    }
}
