using App.Application.Interfaces;
using App.Core.Entities.Core;
using App.WebApi.Models.Sesion;
using App.WebApi.Models.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace App.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class SesionController : ControllerBase
    {
        private readonly ILogger<SesionController> _logger;
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;

        public SesionController(IConfiguration config, ILogger<SesionController> logger, IUnitOfWork unitOfWork)
        {
            _config = config;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Registra una nueva sesión de usuario. Recibe multipart/form-data con campos del DTO y una foto (`foto_inicio`).
        /// </summary>
        [ProducesResponseType(typeof(GenericResponseDto), (int)HttpStatusCode.OK)]
        [HttpPost(Name = "Sesion_Crear")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CrearSesion([FromForm] CrearSesionRequestDto dto)
        {
            try
            {
                // Validar dto
                if (!ModelState.IsValid) return BadRequest(ModelState);

                // Validar que la foto sea requerida
                if (dto.foto_inicio == null || dto.foto_inicio.Length == 0)
                {
                    return BadRequest(new { message = "La foto de inicio es requerida." });
                }

                // Construir entidad SesionUsuario
                var sesion = new SesionUsuario();
                sesion.usuario_id = User.Id();

                if (dto.cliente_id.HasValue)
                {
                    sesion.cliente_id = dto.cliente_id.Value;
                }

                sesion.unidad_id = dto.unidad_id;
                sesion.fecha_inicio = dto.fecha_inicio ?? DateTimeOffset.UtcNow;

                if (!string.IsNullOrEmpty(dto.otros_detalle))
                {
                    sesion.otros_detalle = dto.otros_detalle;
                }

                sesion.SetCreationAudit(DateTimeOffset.UtcNow, User.Id());

                // Preparar stream de la foto (ya validada)
                Stream? fotoStream = null;
                string originalFileName = string.Empty;
                if (dto.foto_inicio != null && dto.foto_inicio.Length > 0)
                {
                    fotoStream = dto.foto_inicio.OpenReadStream();
                    originalFileName = dto.foto_inicio.FileName ?? "foto.jpg";
                }

                var newId = await _unitOfWork.SesionUsuarios.AddAsync(sesion, fotoStream!, originalFileName);

                // Retornar200 OK con GenericResponseDto
                return Ok(new GenericResponseDto { success = true, message = $"Sesion creada: {newId}" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
