namespace App.Application.Interfaces
{
    /// <summary>
    /// Interfaz genérica para repositorios con operaciones de lectura/escritura.
    /// </summary>
    public interface IGenericRepository<T, TKey> : IReadOnlyRepository<T, TKey> where T : class
    {
        Task<TKey> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(TKey id);
    }
}
