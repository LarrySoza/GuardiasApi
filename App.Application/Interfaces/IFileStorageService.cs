namespace App.Application.Interfaces
{
    /// <summary>
    /// Servicio para almacenar archivos relacionados a entidades del dominio.
    /// Implementaciones deben persistir el archivo en el almacenamiento configurado y
    /// devolver la ruta relativa que se puede guardar en la base de datos.
    /// </summary>
    public interface IFileStorageService
    {
        /// <summary>
        /// Guarda el contenido proporcionado en una carpeta asociada a la entidad (entityName).
        /// El nombre del archivo en disco será un GUID más la extensión original extraída de <paramref name="fileName"/>.
        /// Devuelve la ruta relativa (por ejemplo "Entidad/{guid}.ext").
        /// </summary>
        /// <param name="content">Stream con el contenido del archivo.</param>
        /// <param name="fileName">Nombre original del archivo (se utiliza para extraer la extensión).</param>
        /// <param name="entityName">Nombre de la entidad (se usará como carpeta).</param>
        /// <param name="cancellationToken">Token de cancelación opcional.</param>
        /// <returns>Ruta relativa del archivo guardado.</returns>
        Task<string> SaveAsync(Stream content, string fileName, string entityName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Abre un stream de lectura para un archivo previamente almacenado.
        /// El parámetro <paramref name="relativePath"/> debe ser la ruta relativa devuelta por <see cref="SaveAsync"/>,
        /// por ejemplo "Entidad/yyyy/MM/dd/{file.ext}".
        /// </summary>
        /// <param name="relativePath">Ruta relativa del archivo dentro del storage.</param>
        /// <param name="cancellationToken">Token de cancelación opcional.</param>
        /// <returns>Stream de lectura del archivo. El caller es responsable de disponer el stream.</returns>
        Task<Stream> OpenReadAsync(string relativePath, CancellationToken cancellationToken = default);

        /// <summary>
        /// Devuelve la ruta física completa correspondiente a la ruta relativa dentro del storage.
        /// No valida la existencia del archivo; si se necesita validar, usar <see cref="OpenReadAsync"/>.
        /// </summary>
        /// <param name="relativePath">Ruta relativa devuelta por <see cref="SaveAsync"/>.</param>
        /// <returns>Ruta física absoluta en el sistema de archivos.</returns>
        string GetPhysicalPath(string relativePath);
    }
}
