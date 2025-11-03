using App.Application.Interfaces;
using App.Core.Entities;
using App.Core.Entities.Core;
using App.WebApi.Models.Cliente;
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
    public class ClientesController : ControllerBase
    {
        private readonly ILogger<ClientesController> _logger;
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ClientesController(IConfiguration config, ILogger<ClientesController> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _config = config;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtiene un cliente por su identificador.
        /// </summary>
        /// <param name="id">Identificador (GUID) del cliente.</param>
        /// <returns>200 OK con <see cref="ClienteDto"/> o404 si no existe.</returns>
        [ProducesResponseType(typeof(ClienteDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet("{id:guid}", Name = "Admin_Clientes_GetById")]
        public async Task<IActionResult> GetCliente(Guid id)
        {
            try
            {
                var cliente = await _unitOfWork.Clientes.GetByIdAsync(id);
                if (cliente == null) return NotFound();

                var dto = _mapper.Map<ClienteDto>(cliente);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Busca y lista clientes con paginación y filtro por razon_social o ruc.
        /// </summary>
        /// <param name="search">Texto opcional para filtrar por razon_social o ruc.</param>
        /// <param name="page">Número de página.</param>
        /// <param name="pageSize">Tamaño de página.</param>
        /// <returns>200 OK con la paginación de clientes.</returns>
        [ProducesResponseType(typeof(PaginaDatos<ClienteDto>), (int)HttpStatusCode.OK)]
        [HttpGet(Name = "Admin_Clientes_Buscar")]
        public async Task<IActionResult> BuscarCliente([FromQuery] string? search,
                                                     [FromQuery] int page = 1,
                                                     [FromQuery] int pageSize = 20)
        {
            try
            {
                page = Math.Max(1, page);
                pageSize = Math.Clamp(pageSize, 1, 100);

                var resultado = await _unitOfWork.Clientes.GetPagedAsync(search, page, pageSize);

                var dataDto = _mapper.Map<List<ClienteDto>>(resultado.data);

                var paginaDto = new PaginaDatos<ClienteDto>
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
        /// Crea un nuevo cliente.
        /// </summary>
        /// <param name="dto">Datos del cliente a crear.</param>
        /// <returns>201 Created con id del cliente.</returns>
        [ProducesResponseType(typeof(GenericResponseIdDto<Guid>), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpPost(Name = "Admin_Clientes_Crear")]
        public async Task<IActionResult> CrearCliente([FromBody] CrearClienteRequestDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var entity = _mapper.Map<Cliente>(dto);
                entity.SetCreationAudit(DateTimeOffset.UtcNow, User.Id());

                var id = await _unitOfWork.Clientes.AddAsync(entity);

                return CreatedAtRoute("Admin_Clientes_GetById", new { id = id }, new GenericResponseIdDto<Guid>(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Actualiza un cliente existente.
        /// </summary>
        /// <param name="id">Identificador del cliente.</param>
        /// <param name="dto">Datos a actualizar.</param>
        /// <returns>200 OK o 404 si no existe.</returns>
        [ProducesResponseType(typeof(GenericResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpPut("{id:guid}", Name = "Admin_Clientes_Actualizar")]
        public async Task<IActionResult> ActualizarCliente(Guid id, [FromBody] ActualizarClienteRequestDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var cliente = await _unitOfWork.Clientes.GetByIdAsync(id);
                if (cliente == null) return NotFound();

                // Usar AutoMapper para aplicar cambios sobre la entidad
                _mapper.Map(dto, cliente);

                // Auditar la actualización
                cliente.SetUpdateAudit(DateTimeOffset.UtcNow, User.Id());

                await _unitOfWork.Clientes.UpdateAsync(cliente);

                return Ok(new GenericResponseDto { success = true, message = "Cliente actualizado correctamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Elimina (soft delete) un cliente por su identificador.
        /// </summary>
        /// <param name="id">Identificador (GUID) del cliente a eliminar.</param>
        /// <returns>200 OK si se eliminó correctamente,404 si no existe.</returns>
        [ProducesResponseType(typeof(GenericResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpDelete("{id:guid}", Name = "Admin_Clientes_Eliminar")]
        public async Task<IActionResult> EliminarCliente(Guid id)
        {
            try
            {
                var cliente = await _unitOfWork.Clientes.GetByIdAsync(id);
                if (cliente == null) return NotFound();

                await _unitOfWork.Clientes.DeleteAsync(id);

                return Ok(new GenericResponseDto { success = true, message = "Cliente eliminado correctamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
