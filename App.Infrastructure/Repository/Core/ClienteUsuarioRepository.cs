using App.Application.Interfaces.Core;
using App.Core.Entities;
using App.Core.Entities.Core;
using Microsoft.Extensions.Configuration;

namespace App.Infrastructure.Repository.Core
{
    public class ClienteUsuarioRepository : IClienteUsuarioRepository
    {
        private readonly IConfiguration _config;

        public ClienteUsuarioRepository(IConfiguration configuration)
        {
            _config = configuration;
        }

        public Task<(Guid cliente_id, Guid usuario_id)> AddAsync(ClienteUsuario entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync((Guid cliente_id, Guid usuario_id) id)
        {
            throw new NotImplementedException();
        }

        public Task<PaginaDatos<ClienteUsuario>> FindAsync(string? search, int page = 1, int pageSize = 20)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<ClienteUsuario>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ClienteUsuario?> GetByIdAsync((Guid cliente_id, Guid usuario_id) id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(ClienteUsuario entity)
        {
            throw new NotImplementedException();
        }
    }
}
