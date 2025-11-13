using App.Application.Models.Puesto;
using App.Core.Entities;
using App.Core.Entities.Core;

namespace App.Application.Interfaces.Core
{
    // Repositorio para puesto
    public interface IPuestoRepository
    {
        Task<Guid> AddAsync(Puesto entity, List<int> turnosId);
        Task UpdateAsync(Puesto entity, List<int> turnosId);
        Task DeleteAsync(Guid id);

        Task<PuestoConTurnosDto?> GetByIdAsync(Guid id);

        // Devuelve puestos junto con sus turnos (proyección/DTO)
        Task<PaginaDatos<PuestoConTurnosDto>> GetPagedWithTurnosAsync(string? search, int page = 1, int pageSize = 20);
    }
}
