using App.Core.Entities.Core;

namespace App.Application.Interfaces.Core
{
    // Repositorio para cliente_usuario (tiene auditoría)
    public interface IClienteUsuarioRepository : IAutoIdRepository<ClienteUsuario, (Guid cliente_id, Guid usuario_id)>
    {
    }
}
