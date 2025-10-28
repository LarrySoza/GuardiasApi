using App.WebApi.Entities;
using App.WebApi.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace App.WebApi.Controllers
{
    [Authorize(Roles = "Administrador")]
    [ApiController]
    [Route("admin/[controller]")]
    public class AdministrarUsuarioController : ControllerBase
    {
        private readonly ILogger<AdministrarUsuarioController> _logger;
        private readonly IConfiguration _config;

        public AdministrarUsuarioController(IConfiguration config, ILogger<AdministrarUsuarioController> logger)
        {
            _config = config;
            _logger = logger;
        }

        /// <summary>
        /// Crear un usuario
        /// </summary>
        /// <param name="usuario">Informacion del Usuario</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(UsuarioIdDto), (int)HttpStatusCode.OK)]
        [HttpPost(Name = "RegistrarUsuario")]
        public async Task<IActionResult> Registrar([FromBody] UsuarioRegistroDto usuario)
        {
            try
            {
                var _usuarioClass = new UsuarioClass(_config);

                var _id = await _usuarioClass.RegistrarAsync(usuario);

                return Ok(new UsuarioIdDto { id = _id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Cambiar la clave de un usuario
        /// </summary>
        /// <param name="usuarioId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
        [HttpPut("cambiar_clave", Name = "CambiarClaveUsuario")]
        public async Task<IActionResult> CambiarClaveUsuario([FromQuery] Guid usuarioId, CambiarClaveUsuarioDto model)
        {
            try
            {
                var _usuarioClass = new UsuarioClass(_config);

                await _usuarioClass.ActualizarClaveAsync(usuarioId, model.NuevaClave);

                return Ok(new ResponseDto
                {
                    success = true,
                    message = "Configuración actualizada correctamente"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Listar usuarios con paginacion y filtro de busqueda
        /// </summary>
        /// <param name="search"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(PaginaDatosModel<VwUsuarioPerfil>), (int)HttpStatusCode.OK)]
        [HttpGet(Name = "BuscarUsuario")]
        public async Task<IActionResult> BuscarUsuario([FromQuery] string? search,
                                                       [FromQuery] int page = 1,
                                                       [FromQuery] int pageSize = 20)
        {
            try
            {
                var _clsPasajero = new UsuarioClass(_config);
                var _result = await _clsPasajero.BuscarAsync(search, page, pageSize);
                return Ok(_result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Actualizar el perfil de un usuario
        /// </summary>
        /// <param name="usuarioId"></param>
        /// <param name="usuarioActualizaPerfil"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
        [HttpPut("perfil", Name = "ActualizarPerfil")]
        public async Task<IActionResult> ActualizarPerfil([FromQuery] Guid usuarioId, [FromBody] UsuarioActualizaPerfilDto usuarioActualizaPerfil)
        {
            try
            {
                var _usuarioClass = new UsuarioClass(_config);
                await _usuarioClass.ActualizarPerfilAsync(usuarioId, usuarioActualizaPerfil);
                return Ok(new ResponseDto
                {
                    success = true,
                    message = "Perfil actualizado correctamente"
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
