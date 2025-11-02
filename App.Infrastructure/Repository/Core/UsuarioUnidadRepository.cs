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

        public async Task<Guid> AddAsync(UsuarioUnidad entity)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(Guid id)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task DeleteAsync((Guid usuario_id, Guid unidad_id) id)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task<PaginaDatos<UsuarioUnidad>> FindAsync(string? search, int page = 1, int pageSize = 20)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<UsuarioUnidad>> GetAllAsync()
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task<UsuarioUnidad?> GetByIdAsync(Guid id)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task<UsuarioUnidad?> GetByIdAsync((Guid usuario_id, Guid unidad_id) id)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(UsuarioUnidad entity)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        async Task<(Guid usuario_id, Guid unidad_id)> IGenericRepository<UsuarioUnidad, (Guid usuario_id, Guid unidad_id)>.AddAsync(UsuarioUnidad entity)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }
    }
}
