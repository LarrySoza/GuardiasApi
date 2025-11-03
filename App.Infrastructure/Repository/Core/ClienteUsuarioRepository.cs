using App.Application.Interfaces.Core;
using App.Core.Entities;
using App.Core.Entities.Core;
using App.Infrastructure.Database;

namespace App.Infrastructure.Repository.Core
{
    public class ClienteUsuarioRepository : IClienteUsuarioRepository
    {
        private readonly IDbConnectionFactory _dbFactory;

        public ClienteUsuarioRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<(Guid cliente_id, Guid usuario_id)> AddAsync(ClienteUsuario entity)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task AddOrUpdateAsync(ClienteUsuario entity)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task DeleteAsync((Guid cliente_id, Guid usuario_id) id)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task<PaginaDatos<ClienteUsuario>> GetPagedAsync(string? search, int page = 1, int pageSize = 20)
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
