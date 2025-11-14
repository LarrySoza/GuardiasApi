using App.Core.Entities.Core;

namespace App.Application.Interfaces.Core
{
    public interface IUsuarioPuestoRepository
    {
        Task AddAsync(Guid usuarioId, Guid unidadId);
        Task DeleteAsync(Guid usuarioId, Guid unidadId);
        Task<IReadOnlyList<Puesto>> GetAllAsync(Guid usuarioId);
    }
}
