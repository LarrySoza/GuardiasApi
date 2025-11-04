using App.Application.Interfaces;
using App.Core.Entities;
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
    [Route("api/admin/[controller]")]
    public class PanicAlertsController : ControllerBase
    {
        private readonly ILogger<PanicAlertsController> _logger;
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PanicAlertsController(IConfiguration config, ILogger<PanicAlertsController> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _config = config;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtiene una alerta de pánico por su identificador.
        /// </summary>
        /// <param name="id">Identificador (GUID) de la alerta.</param>
        /// <returns>200 OK con <see cref="PanicAlertDto"/> o404 si no existe.</returns>
        [ProducesResponseType(typeof(PanicAlertDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet("{id:guid}", Name = "Admin_PanicAlerts_ObtenerPorId")]
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
        /// Obtiene todas las alertas de una sesión de usuario.
        /// </summary>
        /// <param name="sesionId">Identificador (GUID) de la sesión.</param>
        /// <returns>200 OK con la lista de <see cref="PanicAlertDto"/>.</returns>
        [ProducesResponseType(typeof(List<PanicAlertDto>), (int)HttpStatusCode.OK)]
        [HttpGet("sesiones/{sesionId:guid}", Name = "Admin_PanicAlerts_GetBySesion")]
        public async Task<IActionResult> GetBySesion(Guid sesionId)
        {
            try
            {
                var items = await _unitOfWork.PanicAlerts.GetAllAsync(sesionId);
                var dtos = _mapper.Map<List<PanicAlertDto>>(items);
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Obtiene alertas paginadas filtradas por usuario.
        /// </summary>
        /// <param name="usuarioId">Identificador de usuario.</param>
        /// <param name="estadoId">Filtro opcional por estado (p. ej. "01").</param>
        /// <param name="pagina">Número de página (1-based).</param>
        /// <param name="tamanoPagina">Tamaño de página (máximo100).</param>
        /// <param name="date">Fecha opcional (DateOnly) para filtrar por fecha de alerta.</param>
        /// <returns>200 OK con paginación de <see cref="PanicAlertDto"/>.</returns>
        [ProducesResponseType(typeof(PaginaDatos<PanicAlertDto>), (int)HttpStatusCode.OK)]
        [HttpGet("usuario/{usuarioId:guid}", Name = "Admin_PanicAlerts_GetPagedByUsuario")]
        public async Task<IActionResult> GetPagedByUsuarioId(Guid usuarioId, [FromQuery] string? estadoId = null, [FromQuery] int pagina =1, [FromQuery] int tamanoPagina =20, [FromQuery] DateOnly? date = null)
        {
            try
            {
                pagina = Math.Max(1, pagina);
                tamanoPagina = Math.Clamp(tamanoPagina,1,100);

                var resultado = await _unitOfWork.PanicAlerts.GetPagedByUsuarioIdAsync(usuarioId, estadoId, pagina, tamanoPagina, date);
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

        /// <summary>
        /// Obtiene alertas paginadas filtradas por cliente.
        /// </summary>
        /// <param name="clienteId">Identificador de cliente.</param>
        /// <param name="estadoId">Filtro opcional por estado (p. ej. "01").</param>
        /// <param name="pagina">Número de página (1-based).</param>
        /// <param name="tamanoPagina">Tamaño de página (máximo100).</param>
        /// <param name="date">Fecha opcional (DateOnly) para filtrar por fecha de alerta.</param>
        [ProducesResponseType(typeof(PaginaDatos<PanicAlertDto>), (int)HttpStatusCode.OK)]
        [HttpGet("cliente/{clienteId:guid}", Name = "Admin_PanicAlerts_GetPagedByCliente")]
        public async Task<IActionResult> GetPagedByClienteId(Guid clienteId, [FromQuery] string? estadoId = null, [FromQuery] int pagina =1, [FromQuery] int tamanoPagina =20, [FromQuery] DateOnly? date = null)
        {
            try
            {
                pagina = Math.Max(1, pagina);
                tamanoPagina = Math.Clamp(tamanoPagina,1,100);

                var resultado = await _unitOfWork.PanicAlerts.GetPagedByClienteIdAsync(clienteId, estadoId, pagina, tamanoPagina, date);
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

        /// <summary>
        /// Obtiene alertas paginadas filtradas por unidad.
        /// </summary>
        /// <param name="unidadId">Identificador de unidad.</param>
        /// <param name="estadoId">Filtro opcional por estado (p. ej. "01").</param>
        /// <param name="pagina">Número de página (1-based).</param>
        /// <param name="tamanoPagina">Tamaño de página (máximo100).</param>
        /// <param name="date">Fecha opcional (DateOnly) para filtrar por fecha de alerta.</param>
        [ProducesResponseType(typeof(PaginaDatos<PanicAlertDto>), (int)HttpStatusCode.OK)]
        [HttpGet("unidad/{unidadId:guid}", Name = "Admin_PanicAlerts_GetPagedByUnidad")]
        public async Task<IActionResult> GetPagedByUnidadId(Guid unidadId, [FromQuery] string? estadoId = null, [FromQuery] int pagina =1, [FromQuery] int tamanoPagina =20, [FromQuery] DateOnly? date = null)
        {
            try
            {
                pagina = Math.Max(1, pagina);
                tamanoPagina = Math.Clamp(tamanoPagina,1,100);

                var resultado = await _unitOfWork.PanicAlerts.GetPagedByUnidadIdAsync(unidadId, estadoId, pagina, tamanoPagina, date);
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

        /// <summary>
        /// Elimina (soft delete) una alerta de pánico por su identificador.
        /// </summary>
        /// <param name="id">Identificador de la alerta a eliminar.</param>
        /// <returns>200 OK con mensaje genérico o404 si no existe.</returns>
        [ProducesResponseType(typeof(GenericResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpDelete("{id:guid}", Name = "Admin_PanicAlerts_Eliminar")]
        public async Task<IActionResult> Eliminar(Guid id)
        {
            try
            {
                var existing = await _unitOfWork.PanicAlerts.GetByIdAsync(id);
                if (existing == null) return NotFound();

                await _unitOfWork.PanicAlerts.DeleteAsync(id);
                return Ok(new GenericResponseDto { success = true, message = "Alerta eliminada correctamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Actualiza únicamente el campo 'mensaje' de una alerta de pánico.
        /// </summary>
        /// <param name="id">Identificador de la alerta.</param>
        /// <param name="request">Objeto que contiene el nuevo mensaje.</param>
        /// <returns>200 OK con mensaje genérico o404 si no existe.</returns>
        [ProducesResponseType(typeof(GenericResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpPatch("{id:guid}/mensaje", Name = "Admin_PanicAlerts_ActualizarMensaje")]
        public async Task<IActionResult> ActualizarMensaje(Guid id, [FromBody] UpdateMensajeRequestDto request)
        {
            try
            {
                var existing = await _unitOfWork.PanicAlerts.GetByIdAsync(id);
                if (existing == null) return NotFound();

                await _unitOfWork.PanicAlerts.UpdateMensajeAsync(id, request.mensaje, User.Id());
                return Ok(new GenericResponseDto { success = true, message = "Mensaje actualizado correctamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
