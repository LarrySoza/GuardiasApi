using App.Core.Entities.Core;

namespace App.Application.Interfaces.Core
{
    // Repositorio para `usuario` (operaciones completas), clave Guid
    public interface IUsuarioRepository : IGenericRepository<Usuario, Guid>
    {
    }
}
