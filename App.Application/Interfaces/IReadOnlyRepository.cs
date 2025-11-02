using App.Core.Entities;

namespace App.Application.Interfaces
{
    public interface IReadOnlyRepository<T, TKey> where T : class
    {
        Task<T?> GetByIdAsync(TKey id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<PaginaDatos<T>> FindAsync(string? search, int page = 1, int pageSize = 20);
    }
}
