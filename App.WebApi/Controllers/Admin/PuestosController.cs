using App.Application.Interfaces;
using App.Core.Entities.Core;
using App.WebApi.Models.Puesto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace App.WebApi.Controllers.Admin
{
    [Authorize(Roles = "ADMIN")]
    [ApiController]
    [Route("admin/[controller]")]
    public class PuestosController : ControllerBase
    {
        private readonly ILogger<PuestosController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public PuestosController(ILogger<PuestosController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Obtiene una página de puestos junto con sus turnos.
        /// </summary>
        /// <param name="search">Texto opcional para filtrar por nombre de puesto.</param>
        /// <param name="pagina">Número de página (mínimo 1).</param>
        /// <param name="tamanoPagina">Tamaño de página (entre 1 y 100).</param>
        /// <returns>200 OK con la página de puestos y sus turnos.</returns>
        [ProducesResponseType(typeof(App.Core.Entities.PaginaDatos<App.Application.Models.Puesto.PuestoConTurnosDto>), (int)HttpStatusCode.OK)]
        [HttpGet(Name = "Admin_Puestos_ObtenerPagina")]
        public async Task<IActionResult> GetPaged([FromQuery] string? search, [FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20)
        {
            try
            {
                pagina = Math.Max(1, pagina);
                tamanoPagina = Math.Clamp(tamanoPagina, 1, 100);

                var resultado = await _unitOfWork.Puestos.GetPagedWithTurnosAsync(search, pagina, tamanoPagina);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo puestos paginados");
                throw;
            }
        }

        /// <summary>
        /// Obtiene un puesto por id.
        /// </summary>
        /// <param name="id">Identificador (GUID) del puesto.</param>
        /// <returns>200 OK con el puesto o 404 si no existe.</returns>
        [ProducesResponseType(typeof(Puesto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet("{id:guid}", Name = "Admin_Puestos_ObtenerPorId")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var puesto = await _unitOfWork.Puestos.GetByIdAsync(id);
                if (puesto == null) return NotFound();
                return Ok(puesto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo puesto por id");
                throw;
            }
        }

        /// <summary>
        /// Obtiene todos los puestos de una unidad (sin paginar), incluyendo sus turnos.
        /// </summary>
        /// <param name="unidadId">Identificador (GUID) de la unidad.</param>
        /// <returns>200 OK con la lista de puestos; 200 con lista vacía si no hay puestos.</returns>
        [ProducesResponseType(typeof(IReadOnlyList<App.Application.Models.Puesto.PuestoConTurnosDto>), (int)HttpStatusCode.OK)]
        [HttpGet("unidad/{unidadId:guid}", Name = "Admin_Puestos_ObtenerPorUnidad")]
        public async Task<IActionResult> GetAllByUnidadId(Guid unidadId)
        {
            try
            {
                var lista = await _unitOfWork.Puestos.GetAllByUnidadIdAsync(unidadId);
                return Ok(lista);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo puestos por unidad");
                throw;
            }
        }

        /// <summary>
        /// Obtiene todos los puestos accesibles por un usuario (unidades asignadas y sus descendientes), incluyendo sus turnos.
        /// </summary>
        /// <param name="userId">Identificador (GUID) del usuario.</param>
        /// <returns>200 OK con la lista de puestos; 200 con lista vacía si no hay puestos.</returns>
        [ProducesResponseType(typeof(IReadOnlyList<App.Application.Models.Puesto.PuestoConTurnosDto>), (int)HttpStatusCode.OK)]
        [HttpGet("usuario/{userId:guid}", Name = "Admin_Puestos_ObtenerPorUsuario")]
        public async Task<IActionResult> GetAllByUsuarioId(Guid userId)
        {
            try
            {
                var lista = await _unitOfWork.Puestos.GetAllByUsuarioIdAsync(userId);
                return Ok(lista);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo puestos por usuario");
                throw;
            }
        }

        /// <summary>
        /// Crea un nuevo puesto con la lista de turnos (ids).
        /// </summary>
        /// <param name="dto">Datos para crear el puesto.</param>
        /// <returns>201 Created con el id del nuevo puesto o 400 Bad Request si los datos son inválidos.</returns>
        [ProducesResponseType(typeof(App.WebApi.Models.Shared.GenericResponseIdDto<Guid>), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpPost(Name = "Admin_Puestos_Crear")]
        public async Task<IActionResult> Create([FromBody] CrearPuestoDtoRequest dto)
        {
            try
            {
                if (dto == null) return BadRequest();

                // Construir entidad Puesto y asignar propiedades públicamente
                var puesto = new Puesto
                {
                    unidad_id = dto.unidad_id,
                    nombre = dto.nombre,
                    lat = dto.lat,
                    lng = dto.lng
                };

                // Auditar creación
                puesto.SetCreationAudit(DateTimeOffset.UtcNow, User.Id());

                var turnosId = dto.turnos_ids ?? new List<int>();

                var id = await _unitOfWork.Puestos.AddAsync(puesto, turnosId);

                return CreatedAtRoute("Admin_Puestos_ObtenerPorId", new { id }, new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando puesto");
                throw;
            }
        }

        /// <summary>
        /// Actualiza un puesto existente.
        /// </summary>
        /// <param name="id">Identificador del puesto a actualizar.</param>
        /// <param name="dto">Datos a actualizar (campos opcionales). Si se incluye "turnos_ids":
        /// - null = no tocar las asociaciones de turnos
        /// - lista vacía = eliminar todas las asociaciones
        /// - lista con ids = reemplazar asociaciones por la lista enviada
        /// </param>
        /// <returns>200 OK si se actualizó, 400 si hay error en el request, 404 si no existe.</returns>
        [ProducesResponseType(typeof(App.WebApi.Models.Shared.GenericResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpPut("{id:guid}", Name = "Admin_Puestos_Actualizar")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ActualizarPuestoDtoRequest dto)
        {
            try
            {
                var existing = await _unitOfWork.Puestos.GetByIdAsync(id);
                if (existing == null) return NotFound();

                // Construir entidad Puesto con valores a actualizar
                var puesto = new Puesto
                {
                    unidad_id = existing.unidad_id,
                    nombre = dto.nombre ?? existing.nombre,
                    lat = dto.lat ?? existing.lat,
                    lng = dto.lng ?? existing.lng
                };

                // Establecer id y auditoría de actualización
                puesto.SetId(id);
                puesto.SetUpdateAudit(DateTimeOffset.UtcNow, User.Id());

                var turnos = dto.turnos_ids; // puede ser null

                await _unitOfWork.Puestos.UpdateAsync(puesto, turnos ?? new List<int>());

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando puesto");
                throw;
            }
        }

        /// <summary>
        /// Elimina (soft delete) un puesto por id.
        /// </summary>
        /// <param name="id">Identificador del puesto a eliminar.</param>
        /// <returns>200 OK si se eliminó correctamente, 404 si no existe.</returns>
        [ProducesResponseType(typeof(App.WebApi.Models.Shared.GenericResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpDelete("{id:guid}", Name = "Admin_Puestos_Eliminar")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var existing = await _unitOfWork.Puestos.GetByIdAsync(id);
                if (existing == null) return NotFound();

                await _unitOfWork.Puestos.DeleteAsync(id);

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando puesto");
                throw;
            }
        }
    }
}
