using App.Application.Interfaces;
using App.Core.Entities.Core;
using App.Infrastructure;
using App.WebApi.Models.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace App.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;

        public LoginController(IConfiguration config, ILogger<LoginController> logger, IUnitOfWork unitOfWork)
        {
            _config = config;
            _logger = logger;
            _unitOfWork = unitOfWork;
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
            ClaimsIdentity? _claimsUser = default;

            // Buscar usuario por nombre (LoginDto.usuario puede ser nombre o email según tu lógica)
            var usuario = await _unitOfWork.Usuarios.GetByNameAsync(loginRequest.usuario);

            if (usuario != null)
            {
                // verificar contraseña usando la utilidad Crypto
                if (Crypto.VerifyHashedPassword(usuario.contrasena_hash, loginRequest.clave))
                {
                    var _claims = new List<Claim>
                    {
                        new Claim(JwtConfigManager.ClaimUsuarioId, usuario.id.ToString()),
                        new Claim(JwtConfigManager.ClaimSecurity, usuario.sello_seguridad.ToString())
                    };

                    var _roles = await _unitOfWork.UsuarioRoles.GetAllAsync(usuario.id);

                    foreach (var _rol in _roles)
                    {
                        _claims.Add(new Claim(ClaimTypes.Role, _rol.codigo!));
                    }

                    _claimsUser = new ClaimsIdentity(_claims.ToArray());
                }
            }

            if (_claimsUser != null)
            {
                var _jwtClass = new JwtConfigManager(_config);
                var _configJwt = _jwtClass.LoadConfig();

                var _secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configJwt.SecretKey));
                var _signinCredentials = new SigningCredentials(_secretKey, SecurityAlgorithms.HmacSha256);

                var _now = DateTime.UtcNow;
                var _lifetimeSeconds = _configJwt.LifetimeSeconds;
                var _expires = TimeSpan.FromSeconds(_lifetimeSeconds);
                var _iat = new DateTimeOffset(_now).ToUniversalTime().ToUnixTimeSeconds();

                //Claims estandar segun la norma rfc7519
                var _claims = new List<Claim>()
                {
                    //Identifica el objeto o usuario en nombre del cual fue emitido el JWT
                    new Claim(JwtRegisteredClaimNames.Sub, loginRequest.usuario),

                    //Identificador único del token incluso entre diferente proveedores de servicio.
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

                    //Identifica la marca temporal en qué el JWT fue emitido.
                    new Claim(JwtRegisteredClaimNames.Iat, _iat.ToString(), ClaimValueTypes.Integer64)
                };

                foreach (var item in _claimsUser.Claims)
                {
                    _claims.Add(item);
                }

                var _exp = _now.Add(_expires);

                var tokeOptions = new JwtSecurityToken(
                    issuer: _configJwt.Issuer,
                    audience: _configJwt.Audience,
                    claims: _claims,
                    expires: _exp,
                    signingCredentials: _signinCredentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                return Ok(new LoginResponseDto()
                {
                    access_token = tokenString,
                    token_type = "JWT",
                    expires = _exp
                });
            }
            else
            {
                return Unauthorized(null);
            }
        }
    }
}
