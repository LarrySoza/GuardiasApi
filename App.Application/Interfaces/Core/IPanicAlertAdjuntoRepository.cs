using App.Core.Entities.Core;

namespace App.Application.Interfaces.Core
{
    // Repositorio para panic_alert_adjunto
    public interface IPanicAlertAdjuntoRepository
    {
        Task<Guid> AddAsync(Guid createdBy, Guid panicAlertId, Stream content, string originalFileName, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id);
        Task<PanicAlertAdjunto?> GetByIdAsync(Guid id);
        Task<IReadOnlyList<PanicAlertAdjunto>> GetAllAsync(Guid panicAlertId);
    }
}
