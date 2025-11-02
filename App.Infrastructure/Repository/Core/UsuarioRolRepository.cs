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

        public Task<Guid> AddAsync(UsuarioRol entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync((Guid usuario_id, string rol_id) id)
        {
            throw new NotImplementedException();
        }

        public Task<PaginaDatos<UsuarioRol>> FindAsync(string? search, int page = 1, int pageSize = 20)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<UsuarioRol>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<UsuarioRol?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<UsuarioRol?> GetByIdAsync((Guid usuario_id, string rol_id) id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(UsuarioRol entity)
        {
            throw new NotImplementedException();
        }

        Task<(Guid usuario_id, string rol_id)> IGenericRepository<UsuarioRol, (Guid usuario_id, string rol_id)>.AddAsync(UsuarioRol entity)
        {
            throw new NotImplementedException();
        }
    }
}
