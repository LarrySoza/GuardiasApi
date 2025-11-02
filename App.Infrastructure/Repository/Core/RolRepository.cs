using App.Application.Interfaces.Core;
using App.Core.Entities;
using App.Core.Entities.Core;
using Microsoft.Extensions.Configuration;

namespace App.Infrastructure.Repository.Core
{
    public class RolRepository : IRolRepository
    {
        private readonly IConfiguration _config;

        public RolRepository(IConfiguration config)
        {
            _config = config;
        }

        public Task<PaginaDatos<Rol>> FindAsync(string? search, int page = 1, int pageSize = 20)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Rol>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Rol?> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
