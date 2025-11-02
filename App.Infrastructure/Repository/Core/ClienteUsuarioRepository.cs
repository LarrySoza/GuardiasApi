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

        public async Task<(Guid cliente_id, Guid usuario_id)> AddAsync(ClienteUsuario entity)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task DeleteAsync((Guid cliente_id, Guid usuario_id) id)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task<PaginaDatos<ClienteUsuario>> FindAsync(string? search, int page = 1, int pageSize = 20)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<ClienteUsuario>> GetAllAsync()
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task<ClienteUsuario?> GetByIdAsync((Guid cliente_id, Guid usuario_id) id)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(ClienteUsuario entity)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }
    }
}
