using App.Core.Entities.Core;

namespace App.Application.Interfaces.Core
{
    // Repositorio read-only para tipo_configuracion (catálogo)
    public interface ITipoConfiguracionRepository : IReadAllRepository<TipoConfiguracion, string>
    {
    }
}
