using App.Core.Entities.Core;

namespace App.Application.Interfaces.Core
{
    // Repositorio para panic_alert
    public interface IPanicAlertRepository : IGenericRepository<PanicAlert, Guid>
    {
    }
}
