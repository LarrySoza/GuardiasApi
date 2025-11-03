using App.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace App.WebApi.Controllers.Admin
{
    [Authorize(Roles = "ADMIN")]
    [ApiController]
    [Route("api/admin/[controller]")]
    public class StorageController : ControllerBase
    {
        private readonly ILogger<StorageController> _logger;
        private readonly IFileStorageService _fileStorage;

        public StorageController(ILogger<StorageController> logger, IFileStorageService fileStorage)
        {
            _logger = logger;
            _fileStorage = fileStorage;
        }

        /// <summary>
        /// Devuelve un archivo almacenado en el storage dado su "relativePath" (ruta relativa devuelta por SaveAsync).
        /// Ejemplo de relativePath: "SesionUsuario/2025/01/01/xxxx.jpg". Debe estar URL-encoded cuando se pasa por ruta/consulta.
        /// </summary>
        /// <param name="relativePath">Ruta relativa en el storage (use '/' como separador).</param>
        [HttpGet("file")]
        public async Task<IActionResult> GetFile([FromQuery] string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath)) return BadRequest(new { message = "relativePath es requerido" });

            try
            {
                var stream = await _fileStorage.OpenReadAsync(relativePath);

                // intentar resolver content-type por extension
                var provider = new FileExtensionContentTypeProvider();
                var contentType = "application/octet-stream";
                if (provider.TryGetContentType(relativePath, out var ct)) contentType = ct;

                // nombre de archivo para descarga: tomar el último segmento
                var fileName = relativePath.Replace('/', System.IO.Path.DirectorySeparatorChar);
                fileName = Path.GetFileName(fileName) ?? "file";

                return File(stream, contentType, fileName);
            }
            catch (FileNotFoundException)
            {
                return NotFound();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al abrir archivo: {path}", relativePath);
                throw;
            }
        }
    }
}
