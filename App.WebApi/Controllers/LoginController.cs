using App.Application.Interfaces;
using App.Infrastructure;
using App.WebApi.Configuration;
using App.WebApi.Models.Auth;
using App.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace App.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;

        public LoginController(IConfiguration config, ILogger<LoginController> logger, IUnitOfWork unitOfWork, IAuthService authService)
        {
            _config = config;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        /// <summary>
        /// Autentica un usuario y emite un token JWT si las credenciales son válidas.
        /// </summary>
        /// <param name="loginRequest">Objeto con las credenciales de inicio de sesión (usuario y clave).</param>
        /// <returns>
        /// -200 OK con <see cref="LoginResponseDto"/> que contiene el token cuando la autenticación es exitosa.
        /// -401 Unauthorized cuando las credenciales no son válidas.
        /// </returns>
        /// <remarks>
        /// El token emitido incluye los claims estándar (sub, jti, iat) y los claims personalizados
        /// con el identificador del usuario y el sello de seguridad. La duración del token se obtiene
        /// desde la configuración JWT.
        /// </remarks>
        [ProducesResponseType(typeof(LoginResponseDto), (int)HttpStatusCode.OK)]
        [HttpPost(Name = "Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
        {
            var response = await _authService.AuthenticateAsync(loginRequest);
            if (response == null)
            {
                return Unauthorized(null);
            }

            return Ok(response);
        }
    }
}
