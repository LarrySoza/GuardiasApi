using App.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace App.Infrastructure.Services
{
    /// <summary>
    /// Implementación simple de almacenamiento local en disco.
    /// Guarda archivos bajo la carpeta configurada en `App:StoragePath` dentro de appsettings.
    /// </summary>
    public class FileStorageService : IFileStorageService
    {
        private readonly string _root;
        private readonly ILogger<FileStorageService> _logger;
        private readonly long _maxSizeBytes;

        /// <summary>
        /// Crea una nueva instancia de <see cref="FileStorageService"/>.
        /// </summary>
        /// <param name="config">Configuración para leer la ruta raíz y tamaño máximo.</param>
        /// <param name="logger">Logger para registrar eventos y errores.</param>
        public FileStorageService(IConfiguration config, ILogger<FileStorageService> logger)
        {
            _logger = logger;

            // Obtener valores de configuración sin depender de GetValue<T>()
            _root = config["App:StoragePath"] ?? "storage";

            var maxMbStr = config["App:MaxUploadSizeMb"];
            if (!int.TryParse(maxMbStr, out var maxMb)) maxMb = 100;
            _maxSizeBytes = maxMb * 1024L * 1024L;
        }

        /// <summary>
        /// Guarda un stream en el almacenamiento físico bajo una carpeta asociada a la entidad.
        /// </summary>
        /// <remarks>
        /// - Genera un nombre en disco a partir de un GUID + extensión extraída de <paramref name="fileName"/>
        /// - Crea una subcarpeta por entidad y fecha en formato: <c>entityName/yyyy/MM/dd</c>
        /// - Escribe el archivo en disco y devuelve la ruta relativa que debe almacenarse en la base de datos.
        /// </remarks>
        /// <param name="content">Stream con el contenido del archivo (se lee desde la posición actual o desde0 si es seekable).</param>
        /// <param name="fileName">Nombre original del archivo (se usa para obtener la extensión).</param>
        /// <param name="entityName">Nombre de la entidad (por ejemplo "SesionUsuario") que se utiliza como carpeta raíz dentro del storage.</param>
        /// <param name="cancellationToken">Token de cancelación opcional.</param>
        /// <returns>
        /// Ruta relativa del archivo creado con separadores '/' en el formato:
        /// <c>"{entityName}/{yyyy}/{MM}/{dd}/{guid}{ext}"</c>.
        /// Ejemplo: <c>SesionUsuario/2025/01/31/3f2a1b4e-... .jpg</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">Si <paramref name="content"/> es null.</exception>
        /// <exception cref="ArgumentException">Si <paramref name="fileName"/> o <paramref name="entityName"/> son inválidos o el stream no es legible o está vacío.</exception>
        /// <exception cref="InvalidOperationException">Si no hay permisos para crear carpetas o escribir el archivo.</exception>
        public async Task<string> SaveAsync(Stream content, string fileName, string entityName, CancellationToken cancellationToken = default)
        {
            if (content == null) throw new ArgumentNullException(nameof(content));
            if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentException("El nombre original del archivo es requerido", nameof(fileName));
            if (string.IsNullOrWhiteSpace(entityName)) throw new ArgumentException("El nombre de la entidad es requerido", nameof(entityName));

            if (!content.CanRead) throw new ArgumentException("El stream del archivo no es legible", nameof(content));

            // Si el stream tiene longitud conocida, validar tamaño
            try
            {
                if (content.CanSeek)
                {
                    if (content.Length == 0) throw new ArgumentException("El archivo está vacío", nameof(content));
                    if (content.Length > _maxSizeBytes) throw new ArgumentException($"El archivo excede el tamaño máximo permitido de {_maxSizeBytes} bytes", nameof(content));
                }
            }
            catch (NotSupportedException) { }

            EnsureRootExists();

            var ext = Path.GetExtension(fileName) ?? string.Empty;
            var guid = Guid.NewGuid().ToString();
            var diskFileName = guid + ext;

            // Crear subcarpeta por entidad y por fecha (yyyy/MM/dd)
            var now = DateTime.UtcNow;
            var year = now.ToString("yyyy");
            var month = now.ToString("MM");
            var day = now.ToString("dd");

            var folder = Path.Combine(_root, entityName, year, month, day);
            try
            {
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            }
            catch (UnauthorizedAccessException uaex)
            {
                _logger.LogError(uaex, "No se tienen permisos para crear la carpeta de entidad: {folder}", folder);
                throw new InvalidOperationException($"No se tienen permisos para crear o acceder a la carpeta de entidad: {folder}", uaex);
            }

            var fullPath = Path.Combine(folder, diskFileName);

            try
            {
                // Guardar el archivo
                using (var stream = new FileStream(fullPath, FileMode.CreateNew))
                {
                    // Si el origen es seekable, resetear posición
                    if (content.CanSeek) content.Seek(0, SeekOrigin.Begin);
                    await content.CopyToAsync(stream, cancellationToken);
                }
            }
            catch (UnauthorizedAccessException uaex)
            {
                _logger.LogError(uaex, "No se tienen permisos para escribir el archivo en: {path}", fullPath);
                throw new InvalidOperationException($"No se tienen permisos para escribir el archivo en: {fullPath}", uaex);
            }
            catch (IOException ioex) when (File.Exists(fullPath))
            {
                // raro: si existe, intentar con otro guid
                _logger.LogWarning(ioex, "Archivo ya existe al intentar crear: {path}. Intentando con otro nombre.", fullPath);
                var altFileName = Guid.NewGuid().ToString() + ext;
                fullPath = Path.Combine(folder, altFileName);
                using (var stream = new FileStream(fullPath, FileMode.CreateNew))
                {
                    if (content.CanSeek) content.Seek(0, SeekOrigin.Begin);
                    await content.CopyToAsync(stream, cancellationToken);
                }

                diskFileName = Path.GetFileName(fullPath);
            }

            // Devolver ruta relativa usando separadores '/': entityName/yyyy/MM/dd/{file}
            var relative = Path.Combine(entityName, year, month, day, diskFileName).Replace(Path.DirectorySeparatorChar, '/');
            return relative;
        }

        /// <summary>
        /// Abre un stream de lectura para un archivo previamente almacenado.
        /// </summary>
        /// <remarks>
        /// - Devuelve un <see cref="Stream"/> abierto en modo lectura. El consumidor es responsable de disponer el stream.
        /// - El método verifica la existencia del archivo y lanzará <see cref="FileNotFoundException"/> si no existe.
        /// - En caso de falta de permisos se lanzará <see cref="InvalidOperationException"/> con inner exception.
        /// </remarks>
        /// <param name="relativePath">Ruta relativa devuelta por <see cref="SaveAsync"/>, por ejemplo "SesionUsuario/2025/01/31/{file}.jpg".</param>
        /// <param name="cancellationToken">Token de cancelación opcional (no usado en la implementación local).</param>
        /// <returns>
        /// <see cref="Task{Stream}"/> que contiene un <see cref="Stream"/> de solo lectura apuntando al archivo en disco.
        /// </returns>
        /// <exception cref="ArgumentException">Si <paramref name="relativePath"/> es nulo o vacío.</exception>
        /// <exception cref="FileNotFoundException">Si el archivo no existe en la ruta calculada a partir del storage root.</exception>
        /// <exception cref="InvalidOperationException">Si no hay permisos para leer el archivo.</exception>
        public Task<Stream> OpenReadAsync(string relativePath, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(relativePath)) throw new ArgumentException("La ruta relativa es requerida", nameof(relativePath));

            // Construir la ruta completa a partir del root
            var safeRelative = relativePath.Replace('/', Path.DirectorySeparatorChar).TrimStart(Path.DirectorySeparatorChar);
            var fullPath = Path.Combine(_root, safeRelative);

            if (!File.Exists(fullPath)) throw new FileNotFoundException("El archivo no existe", fullPath);

            try
            {
                var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                return Task.FromResult((Stream)stream);
            }
            catch (UnauthorizedAccessException uaex)
            {
                _logger.LogError(uaex, "No se tienen permisos para leer el archivo en: {path}", fullPath);
                throw new InvalidOperationException($"No se tienen permisos para leer el archivo en: {fullPath}", uaex);
            }
        }

        /// <summary>
        /// Devuelve la ruta física completa correspondiente a la ruta relativa dentro del storage.
        /// </summary>
        /// <remarks>
        /// - No valida la existencia del archivo; para verificar si el archivo existe use <see cref="OpenReadAsync"/>.
        /// - Convierte separadores '/' a los del sistema y concatena con la carpeta raíz configurada.
        /// </remarks>
        /// <param name="relativePath">Ruta relativa devuelta por <see cref="SaveAsync"/>, por ejemplo "Entidad/yyyy/MM/dd/{file.ext}".</param>
        /// <returns>Ruta física absoluta en el sistema de archivos (string).</returns>
        /// <exception cref="ArgumentException">Si <paramref name="relativePath"/> es nulo o vacío.</exception>
        public string GetPhysicalPath(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath)) throw new ArgumentException("La ruta relativa es requerida", nameof(relativePath));
            var safeRelative = relativePath.Replace('/', Path.DirectorySeparatorChar).TrimStart(Path.DirectorySeparatorChar);
            return Path.Combine(_root, safeRelative);
        }

        /// <summary>
        /// Asegura que la carpeta raíz de almacenamiento exista creando la carpeta si es necesario.
        /// </summary>
        /// <exception cref="InvalidOperationException">Cuando no hay permisos para crear o acceder a la carpeta raíz.</exception>
        private void EnsureRootExists()
        {
            try
            {
                if (!Directory.Exists(_root))
                {
                    Directory.CreateDirectory(_root);
                }
            }
            catch (UnauthorizedAccessException uaex)
            {
                _logger.LogError(uaex, "No se tienen permisos para crear la carpeta de storage: {root}", _root);
                throw new InvalidOperationException($"No se tienen permisos para crear o acceder a la carpeta de almacenamiento: {_root}", uaex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al garantizar la existencia de la carpeta de storage: {root}", _root);
                throw;
            }
        }
    }
}
