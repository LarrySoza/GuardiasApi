using App.Application.Interfaces.Core;
using App.Core.Entities;
using App.Core.Entities.Core;
using Microsoft.Extensions.Configuration;

namespace App.Infrastructure.Repository.Core
{
    public class UsuarioEstadoRepository : IUsuarioEstadoRepository
    {
        private readonly IConfiguration _config;

        public UsuarioEstadoRepository(IConfiguration configuration)
        {
            _config = configuration;
        }

        public Task<PaginaDatos<UsuarioEstado>> FindAsync(string? search, int page = 1, int pageSize = 20)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<UsuarioEstado>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<UsuarioEstado?> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
