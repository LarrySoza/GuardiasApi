using App.Core.Entities;
using App.Core.Entities.Core;

namespace App.Application.Interfaces.Core
{
    // Repositorio para unidad
    public interface IUnidadRepository : IGenericAutoIdRepository<Unidad, Guid>
    {
        Task<PaginaDatos<Unidad>> FindAsync(Guid clienteId, string? search, int page = 1, int pageSize = 20);
    }
}
