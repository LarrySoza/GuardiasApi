using App.Application.Interfaces;
using App.Application.Interfaces.Core;
using App.Core.Entities;
using App.Core.Entities.Core;
using Microsoft.Extensions.Configuration;

namespace App.Infrastructure.Repository.Core
{
    public class UsuarioRolRepository : IUsuarioRolRepository
    {
        private readonly IConfiguration _config;

        public UsuarioRolRepository(IConfiguration configuration)
        {
            _config = configuration;
        }

        public async Task<(Guid usuario_id, string rol_id)> AddAsync(UsuarioRol entity)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        async Task<(Guid usuario_id, string rol_id)> IGenericRepository<UsuarioRol, (Guid usuario_id, string rol_id)>.AddAsync(UsuarioRol entity)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task AddOrUpdateAsync(UsuarioRol entity)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(UsuarioRol entity)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task DeleteAsync((Guid usuario_id, string rol_id) id)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task<PaginaDatos<UsuarioRol>> FindAsync(string? search, int page = 1, int pageSize = 20)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task<UsuarioRol?> GetByIdAsync((Guid usuario_id, string rol_id) id)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }
    }
}
