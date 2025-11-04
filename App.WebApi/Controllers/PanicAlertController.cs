using App.Application.Interfaces;
using App.Core.Entities.Core;
using App.WebApi.Models.PanicAlert;
using App.WebApi.Models.Shared;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
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
        /// Crea una nueva alerta de pánico.
        /// </summary>
        /// <param name="dto">Datos para crear la alerta de pánico.</param>
        /// <returns>Devuelve201 y el identificador del recurso creado en caso de éxito, o400 si el modelo no es válido.</returns>
        /// <response code="201">Alerta creada correctamente. Devuelve un objeto con el id creado.</response>
        /// <response code="400">Datos de entrada inválidos.</response>
        [ProducesResponseType(typeof(GenericResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost(Name = "PanicAlert_Crear")]
        public async Task<IActionResult> Crear([FromForm] CrearPanicAlertRequestDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var entity = new PanicAlert
                {
                    sesion_usuario_id = dto.sesion_usuario_id,
                    lat = dto.lat,
                    lng = dto.lng,
                    fecha_hora = dto.fecha_hora ?? DateTimeOffset.UtcNow,
                    // mensaje and estado_id are not provided by DTO; leave defaults (estado_id has default "01")
                };

                // Set audit creation fields
                entity.SetCreationAudit(DateTimeOffset.UtcNow, User.Id());

                var id = await _unitOfWork.PanicAlerts.AddAsync(entity);

                return CreatedAtRoute("PanicAlert_ObtenerPorId", new { id = id }, new { id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Obtiene una alerta de pánico por su identificador.
        /// </summary>
        /// <param name="id">Identificador (GUID) de la alerta de pánico.</param>
        /// <returns>200 OK con <see cref="PanicAlertDto"/> o404 si no existe.</returns>
        [ProducesResponseType(typeof(PanicAlertDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
    }
}
