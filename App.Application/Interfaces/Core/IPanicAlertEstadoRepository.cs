using App.Core.Entities.Core;

namespace App.Application.Interfaces.Core
{
    // Repositorio read-only para panic_alert_estado (catálogo)
    public interface IPanicAlertEstadoRepository : IReadOnlyRepository<PanicAlertEstado, string>
    {
    }
}
