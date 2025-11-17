using App.Application.Interfaces;
using App.WebApi.Models.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace App.WebApi.Controllers
{
    /// <summary>
    /// Endpoints para operaciones del receptor sobre notificaciones de alertas de pánico.
    /// Requiere autenticación del usuario.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PanicAlertNotificacionController : ControllerBase
    {
        private readonly ILogger<PanicAlertNotificacionController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public PanicAlertNotificacionController(ILogger<PanicAlertNotificacionController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Confirma (acepta) una notificación de alerta de pánico por parte del usuario destinatario.
        /// </summary>
        /// <param name="id">Identificador (GUID) de la notificación a confirmar.</param>
        /// <returns>200 OK con resultado genérico, 404 si no existe, 403 si el usuario no es el destinatario.</returns>
        [HttpPost("{id:guid}/confirmar", Name = "PanicAlertNotificacion_Confirmar")]
        [ProducesResponseType(typeof(GenericResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<IActionResult> Confirmar(Guid id)
        {
            try
            {
                var item = await _unitOfWork.PanicAlertNotificaciones.GetByIdAsync(id);
                if (item == null)
                {
                    _logger.LogWarning("Notificación no encontrada. id={Id}", id);
                    return NotFound();
                }

                var usuarioId = User.Id();

                if (item.usuario_notificado_id != usuarioId)
                {
                    _logger.LogWarning("Usuario no autorizado para confirmar notificación. id={Id} usuario={UserId}", id, usuarioId);
                    return Forbid();
                }

                await _unitOfWork.PanicAlertNotificaciones.Confirm(id, usuarioId);

                _logger.LogInformation("Notificación confirmada correctamente. id={Id} usuario={UserId}", id, usuarioId);

                return Ok(new GenericResponseDto { success = true, message = "Notificación confirmada correctamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error confirmando notificación id={Id}", id);
                throw;
            }
        }
    }
}
