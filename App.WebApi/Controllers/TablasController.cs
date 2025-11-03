using App.Application.Interfaces;
using App.Application.Interfaces.Core;
using App.WebApi.Models.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace App.WebApi.Controllers
{
    /// <summary>
    /// Endpoints para obtener catálogos simples (tablas) usados por la UI.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TablasController : ControllerBase
    {
        private readonly ILogger<TablasController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITipoDocumentoRepository _tipoDocumentoRepo;

        public TablasController(ILogger<TablasController> logger, IUnitOfWork unitOfWork, ITipoDocumentoRepository tipoDocumentoRepo)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _tipoDocumentoRepo = tipoDocumentoRepo;
        }

        /// <summary>
        /// Obtiene todos los estados de alive_check.
        /// </summary>
        /// <returns>Lista de <see cref="TablaDto"/> con id y nombre.</returns>
        [ProducesResponseType(typeof(List<TablaDto>), (int)HttpStatusCode.OK)]
        [HttpGet("alive_check_estado", Name = "Tablas_AliveCheckEstados")]
        public async Task<IActionResult> GetAliveCheckEstados()
        {
            try
            {
                var items = await _unitOfWork.AliveCheckEstados.GetAllAsync();
                var dtos = items.Select(i => new TablaDto { id = i.id, nombre = i.nombre }).ToList();
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo alive_check_estado");
                throw;
            }
        }

        /// <summary>
        /// Obtiene todos los tipos de evento de asignación.
        /// </summary>
        /// <returns>Lista de <see cref="TablaDto"/> con id y nombre.</returns>
        [ProducesResponseType(typeof(List<TablaDto>), (int)HttpStatusCode.OK)]
        [HttpGet("asignacion_evento_tipo", Name = "Tablas_AsignacionEventoTipos")]
        public async Task<IActionResult> GetAsignacionEventoTipos()
        {
            try
            {
                var items = await _unitOfWork.AsignacionEventoTipos.GetAllAsync();
                var dtos = items.Select(i => new TablaDto { id = i.id, nombre = i.nombre }).ToList();
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo asignacion_evento_tipo");
                throw;
            }
        }

        /// <summary>
        /// Obtiene todos los tipos de adjunto para panic alert.
        /// </summary>
        /// <returns>Lista de <see cref="TablaDto"/> con id y nombre.</returns>
        [ProducesResponseType(typeof(List<TablaDto>), (int)HttpStatusCode.OK)]
        [HttpGet("panic_alert_adjunto_tipo", Name = "Tablas_PanicAlertAdjuntoTipos")]
        public async Task<IActionResult> GetPanicAlertAdjuntoTipos()
        {
            try
            {
                var items = await _unitOfWork.PanicAlertAdjuntoTipos.GetAllAsync();
                var dtos = items.Select(i => new TablaDto { id = i.id, nombre = i.nombre }).ToList();
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo panic_alert_adjunto_tipo");
                throw;
            }
        }

        /// <summary>
        /// Obtiene todos los estados de panic_alert.
        /// </summary>
        /// <returns>Lista de <see cref="TablaDto"/> con id y nombre.</returns>
        [ProducesResponseType(typeof(List<TablaDto>), (int)HttpStatusCode.OK)]
        [HttpGet("panic_alert_estado", Name = "Tablas_PanicAlertEstados")]
        public async Task<IActionResult> GetPanicAlertEstados()
        {
            try
            {
                var items = await _unitOfWork.PanicAlertEstados.GetAllAsync();
                var dtos = items.Select(i => new TablaDto { id = i.id, nombre = i.nombre }).ToList();
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo panic_alert_estado");
                throw;
            }
        }

        /// <summary>
        /// Obtiene todos los tipos de documento.
        /// </summary>
        /// <returns>Lista de <see cref="TablaDto"/> con id y nombre.</returns>
        [ProducesResponseType(typeof(List<TablaDto>), (int)HttpStatusCode.OK)]
        [HttpGet("tipo_documento", Name = "Tablas_TipoDocumentos")]
        public async Task<IActionResult> GetTipoDocumentos()
        {
            try
            {
                var items = await _tipoDocumentoRepo.GetAllAsync();
                var dtos = items.Select(i => new TablaDto { id = i.id, nombre = i.nombre }).ToList();
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo tipo_documento");
                throw;
            }
        }

        /// <summary>
        /// Obtiene todos los estados de usuario.
        /// </summary>
        /// <returns>Lista de <see cref="TablaDto"/> con id y nombre.</returns>
        [ProducesResponseType(typeof(List<TablaDto>), (int)HttpStatusCode.OK)]
        [HttpGet("usuario_estado", Name = "Tablas_UsuarioEstados")]
        public async Task<IActionResult> GetUsuarioEstados()
        {
            try
            {
                var items = await _unitOfWork.UsuarioEstados.GetAllAsync();
                var dtos = items.Select(i => new TablaDto { id = i.id, nombre = i.nombre }).ToList();
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo usuario_estado");
                throw;
            }
        }

        /// <summary>
        /// Obtiene todos los roles.
        /// </summary>
        /// <returns>Lista de <see cref="TablaDto"/> con id y nombre.</returns>
        [ProducesResponseType(typeof(List<TablaDto>), (int)HttpStatusCode.OK)]
        [HttpGet("rol", Name = "Tablas_Roles")]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                var items = await _unitOfWork.Roles.GetAllAsync();
                var dtos = items.Select(i => new TablaDto { id = i.id, nombre = i.nombre }).ToList();
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo rol");
                throw;
            }
        }
    }
}
