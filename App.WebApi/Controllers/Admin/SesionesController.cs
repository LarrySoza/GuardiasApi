using App.Application.Interfaces;
using App.Core.Entities;
using App.WebApi.Models.Sesion;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace App.WebApi.Controllers.Admin
{
    [Authorize(Roles = "ADMIN")]
    [ApiController]
    [Route("admin/[controller]")]
    public class SesionesController : ControllerBase
    {
        private readonly ILogger<SesionesController> _logger;
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SesionesController(IConfiguration config, ILogger<SesionesController> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _config = config;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Lista sesiones paginadas para un cliente dado (opcionalmente filtradas por fecha).
        /// </summary>
        [ProducesResponseType(typeof(PaginaDatos<SesionUsuarioDto>), (int)HttpStatusCode.OK)]
        [HttpGet("cliente/{clienteId:guid}", Name = "Admin_Sesiones_ObtenerPaginaPorCliente")]
        public async Task<IActionResult> ObtenerPaginaPorCliente(Guid clienteId,
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanoPagina = 20,
            [FromQuery] DateOnly? date = null)
        {
            try
            {
                pagina = Math.Max(1, pagina);
                tamanoPagina = Math.Clamp(tamanoPagina, 1, 100);

                var resultado = await _unitOfWork.SesionUsuarios.GetPagedByClienteIdAsync(clienteId, pagina, tamanoPagina, date);

                var dataDto = _mapper.Map<List<SesionUsuarioDto>>(resultado.data);

                var paginaDto = new PaginaDatos<SesionUsuarioDto>
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
        /// Lista sesiones paginadas para una unidad dada (opcionalmente filtradas por fecha).
        /// </summary>
        [ProducesResponseType(typeof(PaginaDatos<SesionUsuarioDto>), (int)HttpStatusCode.OK)]
        [HttpGet("unidad/{unidadId:guid}", Name = "Admin_Sesiones_ObtenerPaginaPorUnidad")]
        public async Task<IActionResult> ObtenerPaginaPorUnidad(Guid unidadId,
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanoPagina = 20,
            [FromQuery] DateOnly? date = null)
        {
            try
            {
                pagina = Math.Max(1, pagina);
                tamanoPagina = Math.Clamp(tamanoPagina, 1, 100);

                var resultado = await _unitOfWork.SesionUsuarios.GetPagedByUnidadIdAsync(unidadId, pagina, tamanoPagina, date);

                var dataDto = _mapper.Map<List<SesionUsuarioDto>>(resultado.data);

                var paginaDto = new PaginaDatos<SesionUsuarioDto>
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
        /// Lista sesiones paginadas para un usuario dado (opcionalmente filtradas por fecha).
        /// </summary>
        [ProducesResponseType(typeof(PaginaDatos<SesionUsuarioDto>), (int)HttpStatusCode.OK)]
        [HttpGet("usuario/{usuarioId:guid}", Name = "Admin_Sesiones_ObtenerPaginaPorUsuario")]
        public async Task<IActionResult> ObtenerPaginaPorUsuario(Guid usuarioId,
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanoPagina = 20,
            [FromQuery] DateOnly? date = null)
        {
            try
            {
                pagina = Math.Max(1, pagina);
                tamanoPagina = Math.Clamp(tamanoPagina, 1, 100);

                var resultado = await _unitOfWork.SesionUsuarios.GetPagedByUsuarioIdAsync(usuarioId, pagina, tamanoPagina, date);

                var dataDto = _mapper.Map<List<SesionUsuarioDto>>(resultado.data);

                var paginaDto = new PaginaDatos<SesionUsuarioDto>
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
