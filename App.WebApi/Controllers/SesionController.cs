using App.Application.Interfaces;
using App.Core.Entities.Core;
using App.WebApi.Models.Auth;
using App.WebApi.Models.Sesion;
using App.WebApi.Services;
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
        private readonly IAuthService _authService;

        public SesionController(IConfiguration config, ILogger<SesionController> logger, IUnitOfWork unitOfWork, IAuthService authService)
        {
            _config = config;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        /// <summary>
        /// Crea una nueva sesión de usuario y genera un token JWT asociado a la sesión.
        /// </summary>
        /// <remarks>
        /// Este endpoint recibe un formulario multipart/form-data que debe incluir la foto de inicio
        /// y otros campos de la sesión. Requiere autenticación (atributo [Authorize]).
        /// </remarks>
        /// <param name="dto">Datos para crear la sesión, incluyendo la foto de inicio como <see cref="IFormFile"/>.</param>
        /// <returns>
        /// Devuelve un <see cref="LoginResponseDto"/> con el token JWT si la creación y autenticación son exitosas.
        /// En caso de error de validación devuelve 400 Bad Request.
        /// </returns>
        /// <response code="200">Sesión creada y token JWT devuelto en el cuerpo.</response>
        /// <response code="400">Datos inválidos o foto de inicio no proporcionada.</response>
        /// <response code="500">Error interno del servidor.</response>
        [ProducesResponseType(typeof(LoginResponseDto), (int)HttpStatusCode.OK)]
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
                if (dto.puesto_id.HasValue) sesion.puesto_id = dto.puesto_id.Value;
                if (dto.turno_id.HasValue) sesion.turno_id = dto.turno_id.Value;
                sesion.fecha_inicio = DateTimeOffset.UtcNow;

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

                var newId = await _unitOfWork.SesionUsuarios.AddAsync(sesion, fotoStream!, originalFileName, dto.device_token);

                var response = await _authService.AuthenticateAsync(User.UserName(), newId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
