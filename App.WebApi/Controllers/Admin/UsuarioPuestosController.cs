using App.Application.Interfaces;
using App.WebApi.Models.Puesto;
using App.WebApi.Models.Shared;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace App.WebApi.Controllers.Admin
{
    /// <summary>
    /// Controller para gestionar las asignaciones de puestos a usuarios (operaciones administrativas).
    /// Acceso restringido a usuarios con rol "ADMIN".
    /// </summary>
    [Authorize(Roles = "ADMIN")]
    [ApiController]
    [Route("admin/usuarios")]
    public class UsuarioPuestosController : ControllerBase
    {
        private readonly ILogger<UsuarioPuestosController> _logger;
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UsuarioPuestosController(IConfiguration config, ILogger<UsuarioPuestosController> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _config = config;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        /// <summary>
        /// Asigna un 'puesto' a un usuario.
        /// </summary>
        /// <param name="usuarioId">Identificador del usuario (GUID).</param>
        /// <param name="puestoId">Identificador del puesto a asignar (GUID).</param>
        /// <returns>
        /// - 200 OK con <see cref="GenericResponseDto"/> cuando la asignación se realizó correctamente.
        /// - 404 Not Found si el usuario o el puesto no existen.
        /// </returns>
        [ProducesResponseType(typeof(GenericResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpPost("{usuarioId:guid}/puestos/{puestoId:guid}", Name = "Admin_Usuarios_AsignarPuesto")]
        public async Task<IActionResult> AsignarUnidad(Guid usuarioId, Guid puestoId)
        {
            try
            {
                var usuario = await _unitOfWork.Usuarios.GetByIdAsync(usuarioId);
                if (usuario == null) return NotFound();

                var puesto = await _unitOfWork.Puestos.GetByIdAsync(puestoId);
                if (puesto == null) return NotFound();

                await _unitOfWork.UsuarioPuestos.AddAsync(usuarioId, puestoId);

                return Ok(new GenericResponseDto { success = true, message = "Puesto asignado correctamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Remueve la asignación de un 'puesto' a un usuario.
        /// </summary>
        /// <param name="usuarioId">Identificador del usuario (GUID).</param>
        /// <param name="puestoId">Identificador del puesto que será removido (GUID).</param>
        /// <returns>
        /// - 200 OK con <see cref="GenericResponseDto"/> cuando la operación termina correctamente.
        /// - 404 Not Found si el usuario o el puesto no existen.
        /// </returns>
        [ProducesResponseType(typeof(GenericResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpDelete("{usuarioId:guid}/puestos/{puestoId:guid}", Name = "Admin_Usuarios_RemoverPuesto")]
        public async Task<IActionResult> RemoverPuesto(Guid usuarioId, Guid puestoId)
        {
            try
            {
                var usuario = await _unitOfWork.Usuarios.GetByIdAsync(usuarioId);
                if (usuario == null) return NotFound();

                var puesto = await _unitOfWork.Puestos.GetByIdAsync(puestoId);
                if (puesto == null) return NotFound();

                await _unitOfWork.UsuarioPuestos.DeleteAsync(usuarioId, puestoId);

                return Ok(new GenericResponseDto { success = true, message = "Asignación removida correctamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Obtiene la lista de puestos asignados a un usuario.
        /// </summary>
        /// <param name="usuarioId">Identificador del usuario (GUID).</param>
        /// <returns>
        /// - 200 OK con una lista de <see cref="PuestoDto"/> cuando el usuario existe.
        /// - 404 Not Found si el usuario no existe.
        /// </returns>
        [ProducesResponseType(typeof(List<PuestoDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet("{usuarioId:guid}/puestos", Name = "Admin_Usuarios_GetPuestos")]
        public async Task<IActionResult> GetPuestosAsignados(Guid usuarioId)
        {
            try
            {
                var usuario = await _unitOfWork.Usuarios.GetByIdAsync(usuarioId);
                if (usuario == null) return NotFound();

                var puestos = await _unitOfWork.UsuarioPuestos.GetAllAsync(usuarioId);

                // Mapear puestos a DTOs y retornar
                var dtos = puestos.Select(p => _mapper.Map<PuestoDto>(p)).ToList();

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
