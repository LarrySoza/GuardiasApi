using App.Core.Entities.Core;

namespace App.Application.Interfaces.Core
{
    // Repositorio para alive_check
    public interface IAliveCheckRepository : IAutoIdRepository<AliveCheck, Guid>
    {
    }
}
