using App.Core.Entities.Core;

namespace App.Application.Interfaces.Core
{
    // Repositorio read-only para alive_check_estado (catálogo)
    public interface IAliveCheckEstadoRepository : IReadAllRepository<AliveCheckEstado, string>
    {
    }
}
