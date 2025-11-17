using App.Application.Models.PanicAlert;
using App.Core.Entities;

namespace App.Application.Interfaces.Core
{
    // Repositorio para panic_alert_notificacion
    public interface IPanicAlertNotificacionRepository
    {
        Task<List<Guid>> AddAsync(Guid panicAlertNotificacionId, IReadOnlyList<Guid> usuarios);
        Task Confirm(Guid panicAlertNotificacionId, Guid usuarioId);
        Task<PanicAlertNotificacionDto?> GetByIdAsync(Guid id);
        Task<PaginaDatos<PanicAlertNotificacionDto>> GetPagedAsync(Guid panicAlertId, int page = 1, int pageSize = 20);
    }
}
