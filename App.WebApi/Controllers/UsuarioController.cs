
using App.WebApi.Entities;
using App.WebApi.Infrastructure;
using App.WebApi.Models.Shared;
using App.WebApi.Models.Usuario;
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

        public UsuarioController(IConfiguration config, ILogger<UsuarioController> logger)
        {
            _config = config;
            _logger = logger;
        }

        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
        [HttpPut("clave", Name = "CambiarClave")]
        public async Task<IActionResult> ActualizarClave([FromBody] CambiarClaveDto claves)
        {
            try
            {
                var _usuarioClass = new UsuarioClass(_config);

                if (!(await _usuarioClass.ValidarClave(User.Id(), claves.ClaveActual)))
                {
                    throw new Exception("La clave actual es incorrecta");
                }

                await _usuarioClass.ActualizarClaveAsync(User.Id(), claves.ClaveNueva);

                return Ok(new ResponseDto
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

        [ProducesResponseType(typeof(VwUsuarioPerfil), (int)HttpStatusCode.OK)]
        [HttpGet("perfil",Name = "ObtenerPerfil")]
        public async Task<IActionResult> ObtenerPerfil()
        {
            try
            {
                var _usuarioClass = new UsuarioClass(_config);
                var perfil = await _usuarioClass.ConsultarPerfilPorIdAsync(User.Id());
                return Ok(perfil);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
