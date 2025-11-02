using App.Core.Entities.Core;

namespace App.Application.Interfaces.Core
{
    // Repositorio para sesion_usuario
    public interface ISesionUsuarioRepository : IGenericAutoIdRepository<SesionUsuario, Guid>
    {
    }
}
