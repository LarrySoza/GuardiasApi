using App.Core.Entities.Core;

namespace App.Application.Interfaces.Core
{
    public interface IClienteRepository : IAutoIdRepository<Cliente, Guid>
    {
    }
}
