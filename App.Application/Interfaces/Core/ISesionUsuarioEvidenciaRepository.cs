using App.Core.Entities.Core;

namespace App.Application.Interfaces.Core
{
    // Repositorio para sesion_usuario_evidencia
    public interface ISesionUsuarioEvidenciaRepository
    {
        Task<Guid> GetByIdAsync(Guid id);
        Task<IReadOnlyList<SesionUsuarioEvidencia>> GetAllAsync(Guid sesionUsuarioId);
        Task<Guid> AddAsync(Guid createdBy, Guid sesionUsuarioId, Stream content, string originalFileName, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id);
    }
}
