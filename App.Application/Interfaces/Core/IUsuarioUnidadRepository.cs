using App.Core.Entities.Core;

namespace App.Application.Interfaces.Core
{
    // Repositorio para usuario_unidad (tiene auditoría)
    public interface IUsuarioUnidadRepository
    {
        Task AddAsync(Guid usuarioId, Guid unidadId);
        Task DeleteAsync(Guid usuarioId, Guid unidadId);
        Task<IReadOnlyList<Unidad>> GetAllAsync(Guid usuarioId);
    }
}
