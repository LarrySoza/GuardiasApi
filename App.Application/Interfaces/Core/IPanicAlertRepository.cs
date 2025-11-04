using App.Core.Entities;
using App.Core.Entities.Core;

namespace App.Application.Interfaces.Core
{
    // Repositorio para panic_alert
    public interface IPanicAlertRepository
    {
        Task<Guid> AddAsync(PanicAlert entity);
        Task DeleteAsync(Guid id);
        Task<PanicAlert?> GetByIdAsync(Guid id);
        Task<IReadOnlyList<PanicAlert>> GetAllAsync(Guid sesionId);
        Task<PaginaDatos<PanicAlert>> GetPagedByUsuarioIdAsync(Guid usuarioId, string? estadoId, int page = 1, int pageSize = 20, DateOnly? date = null);
        Task<PaginaDatos<PanicAlert>> GetPagedByClienteIdAsync(Guid clienteId, string? estadoId, int page = 1, int pageSize = 20, DateOnly? date = null);
        Task<PaginaDatos<PanicAlert>> GetPagedByUnidadIdAsync(Guid unidadId, string? estadoId, int page = 1, int pageSize = 20, DateOnly? date = null);
        Task UpdateMensajeAsync(Guid id, string? mensaje, Guid? updatedBy = null);
    }
}
