using App.Application.Interfaces;
using App.WebApi.Models.Shared;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace App.WebApi.Controllers
{
    /// <summary>
    /// Endpoints para obtener catálogos simples (tablas) usados por la UI.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class TablasController : ControllerBase
    {
        private readonly ILogger<TablasController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TablasController(ILogger<TablasController> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtiene todos los estados de alive_check.
        /// </summary>
        /// <returns>Lista de <see cref="TablaDto"/> con id y nombre.</returns>
        [ProducesResponseType(typeof(List<TablaDto>), (int)HttpStatusCode.OK)]
        [HttpGet("alive_check_estado", Name = "Tablas_ObtenerAliveCheckEstados")]
        public async Task<IActionResult> ObtenerAliveCheckEstados()
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
        [HttpGet("asignacion_evento_tipo", Name = "Tablas_ObtenerAsignacionEventoTipos")]
        public async Task<IActionResult> ObtenerAsignacionEventoTipos()
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
        [HttpGet("adjunto_tipo", Name = "Tablas_ObtenerAdjuntoTipos")]
        public async Task<IActionResult> ObtenerPanicAlertAdjuntoTipos()
        {
            try
            {
                var items = await _unitOfWork.AdjuntoTipos.GetAllAsync();
                var dtos = items.Select(i => new TablaDto { id = i.id, nombre = i.nombre }).ToList();
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo adjunto_tipo");
                throw;
            }
        }

        /// <summary>
        /// Obtiene todos los estados de panic_alert.
        /// </summary>
        /// <returns>Lista de <see cref="TablaDto"/> con id y nombre.</returns>
        [ProducesResponseType(typeof(List<TablaDto>), (int)HttpStatusCode.OK)]
        [HttpGet("panic_alert_estado", Name = "Tablas_ObtenerPanicAlertEstados")]
        public async Task<IActionResult> ObtenerPanicAlertEstados()
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
        [HttpGet("tipo_documento", Name = "Tablas_ObtenerTipoDocumentos")]
        public async Task<IActionResult> ObtenerTipoDocumentos()
        {
            try
            {
                var items = await _unitOfWork.TipoDocumento.GetAllAsync();
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
        [HttpGet("usuario_estado", Name = "Tablas_ObtenerUsuarioEstados")]
        public async Task<IActionResult> ObtenerUsuarioEstados()
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
        [HttpGet("rol", Name = "Tablas_ObtenerRoles")]
        public async Task<IActionResult> ObtenerRoles()
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

        /// <summary>
        /// Obtiene todos los turnos.
        /// </summary>
        [ProducesResponseType(typeof(List<App.Application.Models.Turno.TurnoDto>), (int)HttpStatusCode.OK)]
        [HttpGet("turno", Name = "Tablas_ObtenerTurnos")]
        public async Task<IActionResult> ObtenerTurnos()
        {
            try
            {
                var items = await _unitOfWork.Turnos.GetAllAsync();
                var dtos = _mapper.Map<List<App.Application.Models.Turno.TurnoDto>>(items);
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo turno");
                throw;
            }
        }
    }
}
