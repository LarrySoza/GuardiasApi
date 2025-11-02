using App.Core.Entities;

namespace App.Application.Interfaces
{
    /// <summary>
    /// Repositorio de solo lectura orientado a búsquedas y paginación para tablas con muchos registros.
    /// </summary>
    public interface ISearchRepository<T, TKey> where T : class
    {
        Task<T?> GetByIdAsync(TKey id);
        Task<PaginaDatos<T>> FindAsync(string? search, int page = 1, int pageSize = 20);
    }
}
