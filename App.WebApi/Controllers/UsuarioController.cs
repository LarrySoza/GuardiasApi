using App.Application.Interfaces;
using App.WebApi.Models.Shared;
using App.WebApi.Models.Unidad;
using App.WebApi.Models.Usuario;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace App.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly ILogger<UsuarioController> _logger;
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UsuarioController(IConfiguration config, ILogger<UsuarioController> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _config = config;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Cambia la contraseña del usuario autenticado.
        /// </summary>
        /// <param name="claves">Objeto con las propiedades:
        /// - contrasena_actual: la contraseña actual del usuario.
        /// - contrasena_nueva: la nueva contraseña a establecer.</param>
        /// <returns>ActionResult con un `GenericResponseDto`. En caso de éxito devuelve `success = true` y un mensaje descriptivo.</returns>
        /// <remarks>
        /// Requiere que el usuario esté autenticado. Si la contraseña actual no coincide
        /// se lanzará una excepción y se registrará el error. El repositorio debe encargarse
        /// del hash de la nueva contraseña y de cualquier actualización de sello de seguridad.
        /// </remarks>
        [ProducesResponseType(typeof(GenericResponseDto), (int)HttpStatusCode.OK)]
        [HttpPatch("me/password", Name = "Usuarios_CambiarContrasena")]
        public async Task<IActionResult> CambiarContrasena([FromBody] CambiarContrasenaDto claves)
        {
            try
            {
                // Validar que la contraseña actual coincida con la almacenada para el usuario autenticado.
                if (!await _unitOfWork.Usuarios.ValidatePasswordAsync(User.Id(), claves.contrasena_actual))
                {
                    throw new Exception("La contraseña actual es incorrecta");
                }

                // Actualizar la contraseña por la nueva proporcionada (se espera que el repositorio haga el hashing correspondiente).
                await _unitOfWork.Usuarios.UpdatePasswordAsync(User.Id(), claves.contrasena_nueva);

                // Retornar respuesta genérica indicando éxito.
                return Ok(new GenericResponseDto()
                {
                    success = true,
                    message = "Contraseña actualizada correctamente"
                });
            }
            catch (Exception ex)
            {
                // Registrar el error y volver a lanzar para que el middleware de la API lo gestione.
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Obtiene las unidades asignadas al usuario autenticado en forma de árbol.
        /// </summary>
        /// <returns>
        /// -200 OK: Devuelve una lista de <see cref="UnidadDto"/> que representan las raíces del árbol.
        /// Cada nodo contiene su colección `children` con los descendientes.
        /// -404 Not Found: Si el usuario autenticado no existe.
        /// </returns>
        /// <remarks>
        /// Flujo de trabajo:
        ///1. Recupera las unidades explícitamente asignadas al usuario desde `IUsuarioUnidadRepository`.
        ///2. Para cada unidad asignada, recorre hacia arriba la jerarquía cargando padres faltantes desde `IUnidadRepository` para asegurar
        /// que el árbol devuelto esté completo (incluye ancestros no asignados).
        ///3. Construye la estructura de árbol (lista de raíces) y la devuelve.
        ///
        /// Nota: Los nodos añadidos como ancestros pueden no estar marcados como asignados; si se requiere
        /// distinguirlos se puede extender `UnidadDto` con una propiedad adicional (por ejemplo `assigned`).
        /// </remarks>
        [ProducesResponseType(typeof(List<UnidadDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet("me/unidades", Name = "Usuarios_ObtenerUnidades")]
        public async Task<IActionResult> ObtenerUnidadesAsignadas()
        {
            try
            {
                var usuario = await _unitOfWork.Usuarios.GetByIdAsync(User.Id());
                if (usuario == null) return NotFound();

                var unidades = await _unitOfWork.UsuarioUnidades.GetAllAsync(User.Id());

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
