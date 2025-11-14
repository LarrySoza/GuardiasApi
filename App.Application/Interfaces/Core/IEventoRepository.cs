using App.Core.Entities.Core;

namespace App.Application.Interfaces.Core
{
    // Repositorio para ocurrencia
    public interface IEventoRepository : IAutoIdRepository<Evento, Guid>
    {
    }
}
