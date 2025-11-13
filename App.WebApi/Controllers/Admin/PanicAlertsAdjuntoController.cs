using App.Application.Interfaces;
using App.Application.Interfaces.Core;
using App.WebApi.Models.PanicAlert;
using App.WebApi.Models.Shared;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace App.WebApi.Controllers.Admin
{
    [Authorize(Roles = "ADMIN")]
    [ApiController]
    [Route("admin/[controller]")]
    public class PanicAlertsAdjuntoController : ControllerBase
    {
        private readonly ILogger<PanicAlertsAdjuntoController> _logger;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public PanicAlertsAdjuntoController(IConfiguration config, ILogger<PanicAlertsAdjuntoController> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _config = config;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtiene un adjunto por su id.
        /// </summary>
        [HttpGet("{id:guid}", Name = "Admin_PanicAlertsAdjunto_ObtenerPorId")]
        [ProducesResponseType(typeof(PanicAlertAdjuntoDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> ObtenerPorId(Guid id)
        {
            try
            {
                var item = await _unitOfWork.PanicAlertAdjuntos.GetByIdAsync(id);
                if (item == null) return NotFound();

                var dto = _mapper.Map<PanicAlertAdjuntoDto>(item);

                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Lista adjuntos de una alerta.
        /// </summary>
        [HttpGet("by-panic/{panicAlertId:guid}", Name = "Admin_PanicAlertsAdjunto_ListByPanic")]
        [ProducesResponseType(typeof(List<PanicAlertAdjuntoDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll(Guid panicAlertId)
        {
            try
            {
                var items = await _unitOfWork.PanicAlertAdjuntos.GetAllAsync(panicAlertId);
                var dtos = _mapper.Map<List<PanicAlertAdjuntoDto>>(items);

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Elimina (soft delete) un adjunto por su identificador.
        /// </summary>
        /// <param name="id">Identificador del adjunto.</param>
        [HttpDelete("{id:guid}", Name = "Admin_PanicAlertsAdjunto_Eliminar")]
        [ProducesResponseType(typeof(GenericResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Eliminar(Guid id)
        {
            try
            {
                var existing = await _unitOfWork.PanicAlertAdjuntos.GetByIdAsync(id);
                if (existing == null) return NotFound();

                await _unitOfWork.PanicAlertAdjuntos.DeleteAsync(id);
                return Ok(new GenericResponseDto { success = true, message = "Adjunto eliminado correctamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
