using App.Core.Entities.Core;

namespace App.Application.Interfaces.Core
{
    // Repositorio para la tabla usuario_rol (tiene auditoría)
    public interface IUsuarioRolRepository : IGenericRepository<UsuarioRol, (Guid usuario_id, string rol_id)>
    {
    }
}
