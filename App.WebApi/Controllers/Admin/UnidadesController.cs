using App.Application.Interfaces;
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
        public async Task<IActionResult> ObtenerUnidad(Guid id)
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
        /// Devuelve las unidades de un cliente en forma de árbol (basado en unidad_id_padre)
        /// </summary>
        /// <param name="clienteId">Identificador del cliente</param>
        /// <returns>200 OK con lista de unidades en estructura de árbol.</returns>
        [ProducesResponseType(typeof(List<UnidadDto>), (int)HttpStatusCode.OK)]
        [HttpGet("cliente/{clienteId:guid}/tree", Name = "Admin_Unidades_ObtenerArbolPorCliente")]
        public async Task<IActionResult> ObtenerArbolUnidadesPorCliente(Guid clienteId)
        {
            try
            {
                // obtener todas las unidades del cliente
                var unidades = await _unitOfWork.Unidades.GetAllByClienteIdAsync(clienteId);
                if (unidades == null || unidades.Count == 0) return Ok(new List<UnidadDto>());

                // mapear a DTOs
                var dtos = unidades.Select(u => _mapper.Map<UnidadDto>(u)).ToList();

                // diccionario para acceso por id
                var dict = dtos.ToDictionary(d => d.id);

                var roots = new List<UnidadDto>();

                // construir árbol
                foreach (var dto in dtos)
                {
                    if (dto.unidad_id_padre.HasValue && dict.TryGetValue(dto.unidad_id_padre.Value, out var parent))
                    {
                        parent.children.Add(dto);
                    }
                    else
                    {
                        // sin padre => raíz
                        roots.Add(dto);
                    }
                }

                return Ok(roots);
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

                // si se especifica padre, validar existencia y mismo cliente
                if (dto.unidad_id_padre.HasValue)
                {
                    var parent = await _unitOfWork.Unidades.GetByIdAsync(dto.unidad_id_padre.Value);
                    if (parent == null) return BadRequest("El padre especificado no existe");
                    if (parent.cliente_id != dto.cliente_id) return BadRequest("El padre debe pertenecer al mismo cliente");
                    // No es necesario validar ciclos al crear una nueva unidad (aún no existe en DB)
                }

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

        /// <summary>
        /// Cambia el padre de una unidad (reparent). Valida que el nuevo padre pertenezca al mismo cliente y que no cree ciclos.
        /// </summary>
        /// <param name="id">Identificador de la unidad a mover.</param>
        /// <param name="dto">Objeto con `parentId` (null para mover a raíz).</param>
        /// <returns>200 OK si se actualizó correctamente,400/409/404 según el error.</returns>
        [ProducesResponseType(typeof(GenericResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [HttpPatch("{id:guid}/parent", Name = "Admin_Unidades_UpdateParent")]
        public async Task<IActionResult> UpdateParent(Guid id, [FromBody] ParentUpdateDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var unidad = await _unitOfWork.Unidades.GetByIdAsync(id);
                if (unidad == null) return NotFound();

                // si parentId no es null, validar existencia y mismo cliente
                if (dto.parentId.HasValue)
                {
                    var parent = await _unitOfWork.Unidades.GetByIdAsync(dto.parentId.Value);
                    if (parent == null) return NotFound();
                    if (parent.cliente_id != unidad.cliente_id) return BadRequest("El padre debe pertenecer al mismo cliente");
                    // evitar que parent sea la propia unidad
                    if (parent.id == unidad.id) return BadRequest("El padre no puede ser la unidad misma");
                    // evitar ciclos
                    var isDesc = await _unitOfWork.Unidades.IsDescendantAsync(unidad.id, dto.parentId.Value);
                    if (isDesc) return Conflict("La asignación de padre crearía un ciclo");
                }

                // aplicar cambio
                unidad.SetParent(dto.parentId);
                unidad.SetUpdateAudit(DateTimeOffset.UtcNow, User.Id());
                await _unitOfWork.Unidades.UpdateAsync(unidad);

                return Ok(new GenericResponseDto { success = true, message = "Padre actualizado correctamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
