using App.Application.Interfaces;
using App.Core.Entities;
using App.Core.Entities.Core;
using App.WebApi.Models.Shared;
using App.WebApi.Models.Unidad;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace App.WebApi.Controllers.Admin
{
    [Authorize(Roles = "ADMIN")]
    [ApiController]
    [Route("api/admin/[controller]")]
    public class UnidadesController : ControllerBase
    {
        private readonly ILogger<UnidadesController> _logger;
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UnidadesController(IConfiguration config, ILogger<UnidadesController> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _config = config;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtiene una unidad por su identificador.
        /// </summary>
        /// <param name="id">Identificador (GUID) de la unidad.</param>
        /// <returns>200 OK con <see cref="UnidadDto"/> o404 si no existe.</returns>
        [ProducesResponseType(typeof(UnidadDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet("{id:guid}", Name = "Admin_Unidades_GetById")]
        public async Task<IActionResult> GetUnidad(Guid id)
        {
            try
            {
                var unidad = await _unitOfWork.Unidades.GetByIdAsync(id);
                if (unidad == null) return NotFound();

                var dto = _mapper.Map<UnidadDto>(unidad);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Busca y lista unidades con paginación y filtro por nombre o direccion.
        /// </summary>
        /// <param name="search">Texto opcional para filtrar por nombre o direccion.</param>
        /// <param name="page">Número de página.</param>
        /// <param name="pageSize">Tamaño de página.</param>
        /// <returns>200 OK con la paginación de unidades.</returns>
        [ProducesResponseType(typeof(PaginaDatos<UnidadDto>), (int)HttpStatusCode.OK)]
        [HttpGet(Name = "Admin_Unidades_Buscar")]
        public async Task<IActionResult> BuscarUnidad([FromQuery] string? search,
                                                      [FromQuery] int page = 1,
                                                      [FromQuery] int pageSize = 20)
        {
            try
            {
                page = Math.Max(1, page);
                pageSize = Math.Clamp(pageSize, 1, 100);

                var resultado = await _unitOfWork.Unidades.FindAsync(search, page, pageSize);

                var dataDto = _mapper.Map<List<UnidadDto>>(resultado.data);

                var paginaDto = new PaginaDatos<UnidadDto>
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
        /// Busca y lista unidades de un cliente específico con paginación y filtro por nombre o direccion.
        /// </summary>
        /// <param name="clienteId">Identificador (GUID) del cliente cuyos unidades se desean listar. Requerido.</param>
        /// <param name="search">Texto opcional para filtrar por nombre o direccion.</param>
        /// <param name="page">Número de página.</param>
        /// <param name="pageSize">Tamaño de página.</param>
        /// <returns>200 OK con la paginación de unidades filtradas por cliente.</returns>
        [ProducesResponseType(typeof(PaginaDatos<UnidadDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpGet("por-cliente", Name = "Admin_Unidades_BuscarPorCliente")]
        public async Task<IActionResult> BuscarUnidadPorCliente([FromQuery] Guid clienteId,
                                                               [FromQuery] string? search,
                                                               [FromQuery] int page = 1,
                                                               [FromQuery] int pageSize = 20)
        {
            try
            {
                if (clienteId == Guid.Empty) return BadRequest("clienteId es requerido");

                page = Math.Max(1, page);
                pageSize = Math.Clamp(pageSize, 1, 100);

                var resultado = await _unitOfWork.Unidades.FindAsync(clienteId, search, page, pageSize);

                var dataDto = _mapper.Map<List<UnidadDto>>(resultado.data);

                var paginaDto = new PaginaDatos<UnidadDto>
                {
                    total = resultado.total,
                    page = resultado.page,
                    pageSize = resultado.pageSize,
                    totalPages = resultado.totalPages,
                    data = dataDto
                };

                return Ok(paginaDto);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Crea una nueva unidad.
        /// </summary>
        /// <param name="dto">Datos de la unidad a crear.</param>
        /// <returns>201 Created con id de la unidad.</returns>
        [ProducesResponseType(typeof(GenericResponseIdDto<Guid>), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpPost(Name = "Admin_Unidades_Crear")]
        public async Task<IActionResult> CrearUnidad([FromBody] CrearUnidadRequestDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var entity = _mapper.Map<Unidad>(dto);
                entity.SetCreationAudit(DateTimeOffset.UtcNow, User.Id());

                var id = await _unitOfWork.Unidades.AddAsync(entity);

                return CreatedAtRoute("Admin_Unidades_GetById", new { id = id }, new GenericResponseIdDto<Guid>(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Actualiza una unidad existente.
        /// </summary>
        /// <param name="id">Identificador de la unidad.</param>
        /// <param name="dto">Datos a actualizar.</param>
        /// <returns>200 OK o404 si no existe.</returns>
        [ProducesResponseType(typeof(GenericResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpPut("{id:guid}", Name = "Admin_Unidades_Actualizar")]
        public async Task<IActionResult> ActualizarUnidad(Guid id, [FromBody] ActualizarUnidadRequestDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var unidad = await _unitOfWork.Unidades.GetByIdAsync(id);
                if (unidad == null) return NotFound();

                _mapper.Map(dto, unidad);
                unidad.SetUpdateAudit(DateTimeOffset.UtcNow, User.Id());

                await _unitOfWork.Unidades.UpdateAsync(unidad);

                return Ok(new GenericResponseDto { success = true, message = "Unidad actualizada correctamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Elimina (soft delete) una unidad por su identificador.
        /// </summary>
        /// <param name="id">Identificador (GUID) de la unidad a eliminar.</param>
        /// <returns>200 OK si se eliminó correctamente,404 si no existe.</returns>
        [ProducesResponseType(typeof(GenericResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpDelete("{id:guid}", Name = "Admin_Unidades_Eliminar")]
        public async Task<IActionResult> EliminarUnidad(Guid id)
        {
            try
            {
                var unidad = await _unitOfWork.Unidades.GetByIdAsync(id);
                if (unidad == null) return NotFound();

                await _unitOfWork.Unidades.DeleteAsync(id);

                return Ok(new GenericResponseDto { success = true, message = "Unidad eliminada correctamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
