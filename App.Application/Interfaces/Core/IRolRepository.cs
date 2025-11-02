using App.Core.Entities.Core;

namespace App.Application.Interfaces.Core
{
    // Repositorio sólo lectura para el catálogo `rol` (datos iniciales)
    public interface IRolRepository : IReadAllRepository<Rol, string>
    {
    }
}
