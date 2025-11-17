using App.Application.Interfaces;
using App.Application.Models.PanicAlert;
using App.Core.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace App.WebApi.Controllers.Admin
{
    /// <summary>
    /// Controlador para gestionar notificaciones de alertas de pánico (administración).
    /// Proporciona endpoints para obtener una notificación por id y listar notificaciones por alerta con paginación.
    /// Acceso restringido a usuarios con rol ADMIN.
    /// </summary>
    [Authorize(Roles = "ADMIN")]
    [ApiController]
    [Route("admin/[controller]")]
    public class PanicAlertsNotificacionController : ControllerBase
    {
        private readonly ILogger<PanicAlertsNotificacionController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PanicAlertsNotificacionController(ILogger<PanicAlertsNotificacionController> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtiene una notificación de alerta de pánico por su identificador.
        /// </summary>
        /// <param name="id">Identificador (GUID) de la notificación.</param>
        /// <returns>200 OK con <see cref="PanicAlertNotificacionDto"/> o 404 si no existe.</returns>
        [HttpGet("{id:guid}", Name = "Admin_PanicAlertsNotificacion_ObtenerPorId")]
        [ProducesResponseType(typeof(PanicAlertNotificacionDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var item = await _unitOfWork.PanicAlertNotificaciones.GetByIdAsync(id);
                if (item == null)
                {
                    _logger.LogWarning("Notificación de alerta de pánico no encontrada. id={Id}", id);
                    return NotFound();
                }

                _logger.LogInformation("Notificación de alerta de pánico obtenida correctamente. id={Id}", id);
                return Ok(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo notificación de alerta de pánico por id={Id}", id);
                throw;
            }
        }

        /// <summary>
        /// Obtiene notificaciones asociadas a una alerta de pánico (paginado).
        /// </summary>
        /// <param name="panicAlertId">Identificador (GUID) de la alerta de pánico.</param>
        /// <param name="page">Número de página (>=1).</param>
        /// <param name="pageSize">Tamaño de página (1-100).</param>
        /// <returns>200 OK con objeto <see cref="PaginaDatos{PanicAlertNotificacionDto}"/>.</returns>
        [HttpGet("by-panic/{panicAlertId:guid}", Name = "Admin_PanicAlertsNotificacion_GetPagedByPanic")]
        [ProducesResponseType(typeof(PaginaDatos<PanicAlertNotificacionDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPagedByPanic(Guid panicAlertId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                page = Math.Max(1, page);
                pageSize = Math.Clamp(pageSize, 1, 100);

                var result = await _unitOfWork.PanicAlertNotificaciones.GetPagedAsync(panicAlertId, page, pageSize);

                _logger.LogInformation("Listado de notificaciones para alerta {PanicAlertId} obtenido. page={Page} pageSize={PageSize} total={Total}", panicAlertId, page, pageSize, result.total);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo lista paginada de notificaciones para alerta id={PanicAlertId}", panicAlertId);
                throw;
            }
        }
    }
}
