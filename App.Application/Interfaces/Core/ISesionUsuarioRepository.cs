using App.Core.Entities;
using App.Core.Entities.Core;

namespace App.Application.Interfaces.Core
{
    // Repositorio para sesion_usuario
    public interface ISesionUsuarioRepository
    {
        Task<SesionUsuario?> GetByIdAsync(Guid id);
        Task<PaginaDatos<SesionUsuario>> GetPagedByUsuarioIdAsync(Guid usuarioId, int page = 1, int pageSize = 20, DateOnly? date = null);
        Task<PaginaDatos<SesionUsuario>> GetPagedByClienteIdAsync(Guid clienteId, int page = 1, int pageSize = 20, DateOnly? date = null);
        Task<PaginaDatos<SesionUsuario>> GetPagedByUnidadIdAsync(Guid unidadId, int page = 1, int pageSize = 20, DateOnly? date = null);
        Task<Guid> AddAsync(SesionUsuario entity, Stream content, string originalFileName, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id);
    }
}
