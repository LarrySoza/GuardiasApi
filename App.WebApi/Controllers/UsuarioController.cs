using App.Application.Interfaces;
using App.WebApi.Models.Shared;
using App.WebApi.Models.Usuario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace App.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly ILogger<UsuarioController> _logger;
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;

        public UsuarioController(IConfiguration config, ILogger<UsuarioController> logger, IUnitOfWork unitOfWork)
        {
            _config = config;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [ProducesResponseType(typeof(GenericResponseDto), (int)HttpStatusCode.OK)]
        [HttpPatch("me/password", Name = "Usuarios_CambiarPassword")]
        public async Task<IActionResult> CambiarPassword([FromBody] CambiarContrasenaDto claves)
        {
            try
            {
                if (!await _unitOfWork.Usuarios.ValidatePasswordAsync(User.Id(), claves.contrasena_actual))
                {
                    throw new Exception("La clave actual es incorrecta");
                }

                await _unitOfWork.Usuarios.UpdatePasswordAsync(User.Id(), claves.contrasena_nueva);

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
