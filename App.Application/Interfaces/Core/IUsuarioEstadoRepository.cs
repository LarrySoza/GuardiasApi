using App.Core.Entities.Core;

namespace App.Application.Interfaces.Core
{
    // Repositorio read-only para usuario_estado (catálogo)
    public interface IUsuarioEstadoRepository : IReadAllRepository<UsuarioEstado, string>
    {
    }
}
