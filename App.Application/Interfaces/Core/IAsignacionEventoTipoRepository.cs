using App.Core.Entities.Core;

namespace App.Application.Interfaces.Core
{
    // Repositorio read-only para asignacion_evento_tipo (catálogo)
    public interface IAsignacionEventoTipoRepository : IReadAllRepository<AsignacionEventoTipo, string>
    {
    }
}
