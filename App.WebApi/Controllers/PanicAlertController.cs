using App.Application.Interfaces;
using App.Core.Entities;
using App.Core.Entities.Core;
using App.WebApi.Models.PanicAlert;
using App.WebApi.Models.Shared;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace App.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PanicAlertController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<PanicAlertController> _logger;

        public PanicAlertController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PanicAlertController> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Crea una nueva alerta de pánico para la sesión del usuario autenticado.
        /// </summary>
        /// <param name="dto">Datos para crear la alerta de pánico. Solo se consideran `lat` y `lng` del cuerpo de la petición.</param>
        /// <remarks>
        /// - El `sesion_usuario_id` se obtiene a partir del token (claim de sesión) y no puede ser enviado por el cliente.
        /// - La fecha se establece en UTC en el servidor (`DateTimeOffset.UtcNow`).
        /// - Se registran notificaciones para los usuarios relacionados si existen.
        /// </remarks>
        /// <returns>
        /// - 201 Created con un objeto <see cref="GenericResponseIdDto{Guid}"/> que contiene el id de la alerta creada.
        /// - 400 Bad Request si el modelo no es válido.
        /// </returns>
        /// <response code="201">Alerta creada correctamente. Devuelve un objeto con el id creado.</response>
        /// <response code="400">Datos de entrada inválidos.</response>
        [ProducesResponseType(typeof(GenericResponseIdDto<Guid>), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpPost(Name = "PanicAlert_Crear")]
        public async Task<IActionResult> Crear([FromBody] CrearPanicAlertRequestDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var entity = new PanicAlert
                {
                    sesion_usuario_id = User.SesionId(),
                    lat = dto.lat,
                    lng = dto.lng,
                    fecha_hora = DateTimeOffset.UtcNow,
                    // mensaje and estado_id are not provided by DTO; leave defaults (estado_id has default "01")
                };

                // Set audit creation fields
                entity.SetCreationAudit(DateTimeOffset.UtcNow, User.Id());

                var id = await _unitOfWork.PanicAlerts.AddAsync(entity);

                var _usuariosForNotificacion = await _unitOfWork.Usuarios.GetUsersForPanicAlertNotificacionAsync(entity.sesion_usuario_id);

                if (_usuariosForNotificacion != null && _usuariosForNotificacion.Count > 0)
                {
                    await _unitOfWork.PanicAlertNotificaciones.AddAsync(id, _usuariosForNotificacion);
                }

                return CreatedAtRoute("PanicAlert_ObtenerPorId", new { id = id }, new GenericResponseIdDto<Guid>(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        [ProducesResponseType(typeof(PanicAlertDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet("{id:guid}", Name = "PanicAlert_ObtenerPorId")]
        public async Task<IActionResult> ObtenerPorId(Guid id)
        {
            try
            {
                var item = await _unitOfWork.PanicAlerts.GetByIdAsync(id);
                if (item == null) return NotFound();

                var dto = _mapper.Map<PanicAlertDto>(item);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Cancela una alerta de pánico estableciendo su estado a '03' (cancelado).
        /// </summary>
        /// <param name="id">Identificador (GUID) de la alerta a cancelar.</param>
        /// <returns>200 OK con mensaje genérico o404 si no existe.</returns>
        [ProducesResponseType(typeof(GenericResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpPost("{id:guid}/cancelar", Name = "PanicAlert_Cancelar")]
        public async Task<IActionResult> Cancelar(Guid id)
        {
            try
            {
                var item = await _unitOfWork.PanicAlerts.GetByIdAsync(id);
                if (item == null) return NotFound();

                await _unitOfWork.PanicAlerts.UpdateEstadoAsync(id, "03", User.Id());

                return Ok(new GenericResponseDto { success = true, message = "Alerta cancelada correctamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        [ProducesResponseType(typeof(PaginaDatos<PanicAlertDto>), (int)HttpStatusCode.OK)]
        [HttpGet(Name = "PanicAlert_ObtenerPaginado")]
        public async Task<IActionResult> ObtenerPaginado([FromQuery] string? estadoId = null, [FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20, [FromQuery] DateOnly? date = null)
        {
            try
            {
                pagina = Math.Max(1, pagina);
                tamanoPagina = Math.Clamp(tamanoPagina, 1, 100);

                var resultado = await _unitOfWork.PanicAlerts.GetPagedByUsuarioIdAsync(User.Id(), estadoId, pagina, tamanoPagina, date);
                var dataDto = _mapper.Map<List<PanicAlertDto>>(resultado.data);

                var paginaDto = new PaginaDatos<PanicAlertDto>
                {
                    total = resultado.total,
                    page = resultado.page,
                    pageSize = resultado.pageSize,
                    totalPages = resultado.totalPages,
                    data = dataDto
                };

                return Ok(paginaDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
