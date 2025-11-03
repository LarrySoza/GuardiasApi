using App.Application.Interfaces;
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
    [Route("api/admin/usuarios")]
    public class UsuarioUnidadesController : ControllerBase
    {
        private readonly ILogger<UsuarioUnidadesController> _logger;
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UsuarioUnidadesController(IConfiguration config, ILogger<UsuarioUnidadesController> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _config = config;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Asigna una unidad a un usuario (crea relación usuario_unidad).
        /// </summary>
        [ProducesResponseType(typeof(GenericResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpPost("{usuarioId:guid}/unidades/{unidadId:guid}", Name = "Admin_Usuarios_AsignarUnidad")]
        public async Task<IActionResult> AsignarUnidad(Guid usuarioId, Guid unidadId)
        {
            try
            {
                var usuario = await _unitOfWork.Usuarios.GetByIdAsync(usuarioId);
                if (usuario == null) return NotFound();

                var unidad = await _unitOfWork.Unidades.GetByIdAsync(unidadId);
                if (unidad == null) return NotFound();

                await _unitOfWork.UsuarioUnidades.AddAsync(usuarioId, unidadId);

                return Ok(new GenericResponseDto { success = true, message = "Unidad asignada correctamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Remueve (soft delete) la asignación de una unidad a un usuario.
        /// </summary>
        [ProducesResponseType(typeof(GenericResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpDelete("{usuarioId:guid}/unidades/{unidadId:guid}", Name = "Admin_Usuarios_RemoverUnidad")]
        public async Task<IActionResult> RemoverUnidad(Guid usuarioId, Guid unidadId)
        {
            try
            {
                var usuario = await _unitOfWork.Usuarios.GetByIdAsync(usuarioId);
                if (usuario == null) return NotFound();

                var unidad = await _unitOfWork.Unidades.GetByIdAsync(unidadId);
                if (unidad == null) return NotFound();

                await _unitOfWork.UsuarioUnidades.DeleteAsync(usuarioId, unidadId);

                return Ok(new GenericResponseDto { success = true, message = "Asignación removida correctamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Obtiene las unidades asignadas a un usuario en forma de árbol, incluyendo ancestros no asignados.
        /// </summary>
        [ProducesResponseType(typeof(List<UnidadDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet("{usuarioId:guid}/unidades", Name = "Admin_Usuarios_GetUnidades")]
        public async Task<IActionResult> GetUnidadesAsignadas(Guid usuarioId)
        {
            try
            {
                var usuario = await _unitOfWork.Usuarios.GetByIdAsync(usuarioId);
                if (usuario == null) return NotFound();

                var unidades = await _unitOfWork.UsuarioUnidades.GetAllAsync(usuarioId);

                // Mapear unidades asignadas a DTOs
                var dtos = unidades.Select(u => _mapper.Map<UnidadDto>(u)).ToList();

                // Diccionario para búsqueda por id
                var dict = dtos.ToDictionary(d => d.id);

                // Asegurar que se incluyan ancestros: para cada unidad asignada, recorrer padres y cargar los faltantes
                foreach (var assigned in unidades)
                {
                    var parentId = assigned.unidad_id_padre;
                    while (parentId.HasValue && !dict.ContainsKey(parentId.Value))
                    {
                        var parent = await _unitOfWork.Unidades.GetByIdAsync(parentId.Value);
                        if (parent == null) break; // padre no encontrado o eliminado

                        var parentDto = _mapper.Map<UnidadDto>(parent);

                        // Añadir al diccionario y a la lista
                        dict[parentDto.id] = parentDto;
                        dtos.Add(parentDto);

                        // Continuar subiendo por la jerarquía
                        parentId = parent.unidad_id_padre;
                    }
                }

                // Construir el árbol usando la lista completa de DTOs
                var dictById = dtos.ToDictionary(d => d.id);
                var roots = new List<UnidadDto>();

                foreach (var dto in dtos)
                {
                    if (dto.unidad_id_padre.HasValue && dictById.TryGetValue(dto.unidad_id_padre.Value, out var parent))
                    {
                        parent.children.Add(dto);
                    }
                    else
                    {
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
    }
}
