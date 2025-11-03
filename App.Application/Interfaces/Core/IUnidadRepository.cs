using App.Core.Entities.Core;

namespace App.Application.Interfaces.Core
{
    // Repositorio para unidad
    public interface IUnidadRepository
    {
        Task<Guid> AddAsync(Unidad entity);
        Task UpdateAsync(Unidad entity);
        Task DeleteAsync(Guid id);

        Task<Unidad?> GetByIdAsync(Guid id);

        // Retorna todas las unidades de un cliente (sin paginar) - útil para construir árboles
        Task<IReadOnlyList<Unidad>> GetAllByClienteIdAsync(Guid clienteId);

        // Verifica si possibleDescendantId es descendiente de unidadId (true = yes)
        Task<bool> IsDescendantAsync(Guid unidadId, Guid possibleDescendantId);
    }
}
