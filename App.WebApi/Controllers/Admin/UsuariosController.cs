using App.Application.Interfaces;
using App.Core.Entities.Core;
using App.Infrastructure;
using App.WebApi.Models.Shared;
using App.WebApi.Models.Usuario;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace App.WebApi.Controllers.Admin
{
    /// <summary>
    /// Controller para operaciones administrativas sobre usuarios.
    /// Acceso restringido a usuarios con el rol "ADMIN".
    /// </summary>
    [Authorize(Roles = "ADMIN")]
    [ApiController]
    [Route("api/admin/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly ILogger<UsuariosController> _logger;
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UsuariosController(IConfiguration config, ILogger<UsuariosController> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _config = config;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtiene los datos públicos de un usuario por su identificador, incluyendo la lista de roles asignados.
        /// </summary>
        /// <param name="id">Identificador (GUID) del usuario.</param>
        /// <returns>
        /// - 200 OK con <see cref="UsuarioDto"/> cuando el usuario existe.
        /// - 404 Not Found cuando no se encuentra el usuario.
        /// </returns>
        /// <response code="200">Usuario encontrado y retornado como <see cref="UsuarioDto"/>.</response>
        /// <response code="404">Usuario no encontrado.</response>
        /// <response code="500">Error interno del servidor.</response>
        /// <remarks>
        /// Este endpoint no expone información sensible (por ejemplo, contrasena_hash o sello_seguridad).
        /// Los roles se obtienen desde el repositorio de relaciones usuario-rol y se mapean a <see cref="RolDto"/>.
        /// </remarks>
        [ProducesResponseType(typeof(UsuarioDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet("{id:guid}", Name = "Admin_Usuarios_GetById")]
        public async Task<IActionResult> GetUsuario(Guid id)
        {
            try
            {
                var usuario = await _unitOfWork.Usuarios.GetByIdAsync(id);
                if (usuario == null) return NotFound();

                // Mapear entidad Usuario -> UsuarioDto
                var dto = _mapper.Map<UsuarioDto>(usuario);

                // Cargar roles y mapear a RolDto
                var roles = await _unitOfWork.UsuarioRoles.GetAllAsync(id);
                dto.roles = _mapper.Map<List<RolDto>>(roles);

                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Crea un nuevo usuario con los datos proporcionados y retorna su identificador.
        /// </summary>
        /// <param name="dto">Datos para la creación del usuario.</param>
        /// <returns>El <see cref="Guid"/> del usuario creado.</returns>
        /// <response code="201">Usuario creado correctamente; devuelve el id (Guid) y Location hacia el recurso.</response>
        /// <response code="400">Modelo inválido.</response>
        [ProducesResponseType(typeof(GenericResponseId<Guid>), (int)HttpStatusCode.OK)]
        [HttpPost(Name = "Admin_Usuarios_Crear")]
        public async Task<IActionResult> CrearUsuario([FromBody] CrearUsuarioRequestDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var _usuario = _mapper.Map<Usuario>(dto);

                // generar hash y sello
                _usuario.contrasena_hash = Crypto.HashPassword(dto.contrasena);
                _usuario.sello_seguridad = Guid.NewGuid();
                _usuario.SetCreationAudit(User.Id());

                // convertir roles (si vienen ids)
                var roles = (dto.roles_id ?? new List<string>()).Select(id => new Rol { id = id }).ToList();

                var id = await _unitOfWork.Usuarios.AddAsync(_usuario, roles);

                return CreatedAtRoute("Admin_Usuarios_GetById", id, new GenericResponseId<Guid>(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Actualiza la contraseña de un usuario identificado por su <paramref name="id"/>.
        /// Este endpoint es para uso administrativo; cambia la clave del usuario objetivo.
        /// </summary>
        /// <param name="id">Identificador (Guid) del usuario cuya contraseña será actualizada.</param>
        /// <param name="clave">Objeto que contiene la nueva contraseña (<see cref="NuevaContrasenaDto"/>).</param>
        /// <returns>Un <see cref="GenericResponseDto"/> indicando el resultado de la operación.</returns>
        /// <response code="200">Contraseña actualizada correctamente.</response>
        /// <response code="404">Usuario no encontrado (cuando la ruta no casó con un guid válido o no existe).</response>
        [ProducesResponseType(typeof(GenericResponseDto), (int)HttpStatusCode.OK)]
        [HttpPatch("{id:guid}/password", Name = "Admin_Usuarios_ActualizarPassword")]
        public async Task<IActionResult> CambiarPassword(Guid id, [FromBody] NuevaContrasenaDto clave)
        {
            try
            {
                await _unitOfWork.Usuarios.UpdatePasswordAsync(id, clave.contrasena_nueva);

                return Ok(new GenericResponseDto()
                {
                    success = true,
                    message = "Clave actualizada correctamente"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
