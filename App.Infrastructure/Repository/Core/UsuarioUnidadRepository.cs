using App.Application.Interfaces;
using App.Application.Interfaces.Core;
using App.Core.Entities;
using App.Core.Entities.Core;
using Microsoft.Extensions.Configuration;

namespace App.Infrastructure.Repository.Core
{
    public class UsuarioUnidadRepository : IUsuarioUnidadRepository
    {
        private readonly IConfiguration _config;

        public UsuarioUnidadRepository(IConfiguration configuration)
        {
            _config = configuration;
        }

        public Task<Guid> AddAsync(UsuarioUnidad entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync((Guid usuario_id, Guid unidad_id) id)
        {
            throw new NotImplementedException();
        }

        public Task<PaginaDatos<UsuarioUnidad>> FindAsync(string? search, int page = 1, int pageSize = 20)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<UsuarioUnidad>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<UsuarioUnidad?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<UsuarioUnidad?> GetByIdAsync((Guid usuario_id, Guid unidad_id) id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(UsuarioUnidad entity)
        {
            throw new NotImplementedException();
        }

        Task<(Guid usuario_id, Guid unidad_id)> IGenericRepository<UsuarioUnidad, (Guid usuario_id, Guid unidad_id)>.AddAsync(UsuarioUnidad entity)
        {
            throw new NotImplementedException();
        }
    }
}
