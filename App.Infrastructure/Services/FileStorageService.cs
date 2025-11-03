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

        public FileStorageService(IConfiguration config, ILogger<FileStorageService> logger)
        {
            _logger = logger;

            // Obtener valores de configuración sin depender de GetValue<T>()
            _root = config["App:StoragePath"] ?? "storage";

            var maxMbStr = config["App:MaxUploadSizeMb"];
            if (!int.TryParse(maxMbStr, out var maxMb)) maxMb = 100;
            _maxSizeBytes = maxMb * 1024L * 1024L;
        }

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

        public string GetPhysicalPath(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath)) throw new ArgumentException("La ruta relativa es requerida", nameof(relativePath));
            var safeRelative = relativePath.Replace('/', Path.DirectorySeparatorChar).TrimStart(Path.DirectorySeparatorChar);
            return Path.Combine(_root, safeRelative);
        }

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
