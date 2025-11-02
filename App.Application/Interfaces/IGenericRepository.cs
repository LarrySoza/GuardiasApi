namespace App.Application.Interfaces
{
    /// <summary>
    /// Interfaz genérica para repositorios que devuelven un id de tipo TKey al agregar una entidad.
    /// </summary>
    public interface IGenericRepository<T, TKey> : ISearchRepository<T, TKey> where T : class
    {
        Task<TKey> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(TKey id);
    }

    /// <summary>
    /// Interfaz genérica para repositorios que no devuelven un id al agregar una entidad.
    /// </summary>
    public interface IGenericRepository<T> : IReadAllRepository<T, string> where T : class
    {
        Task AddAsync(T entity);
        Task AddOrUpdateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(string id);
    }
}
