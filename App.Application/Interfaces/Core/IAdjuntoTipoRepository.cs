using App.Core.Entities.Core;

namespace App.Application.Interfaces.Core
{
    // Repositorio read-only para panic_alert_adjunto_tipo (catálogo)
    public interface IAdjuntoTipoRepository : IReadAllRepository<AdjuntoTipo, string>
    {
    }
}
