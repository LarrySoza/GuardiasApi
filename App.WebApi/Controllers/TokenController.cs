using App.WebApi.Infrastructure;
using App.WebApi.Models.Usuario;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace App.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly ILogger<TokenController> _logger;
        private readonly IConfiguration _config;

        public TokenController(IConfiguration config, ILogger<TokenController> logger)
        {
            _config = config;
            _logger = logger;
        }

        /// <summary>
        /// Obtener el token JWT
        /// </summary>
        /// <param name="user">Datos de inicio de sesión</param>
        [ProducesResponseType(typeof(TokenDto), (int)HttpStatusCode.OK)]
        [HttpPost(Name = "ObtenerToken")]
        public async Task<IActionResult> Login([FromBody] Login user)
        {
            if (user == null)
            {
                return BadRequest("Solicitud incorrecta.");
            }

            var _loginClass = new LoginClass(_config);

            var _claimsUser = await _loginClass.GetIdentityAsync(user);

            if (_claimsUser != null)
            {
                var _jwtClass = new JwtClass(_config);
                var _configJwt = _jwtClass.LeerConfig();

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
                    new Claim(JwtRegisteredClaimNames.Sub, user.usuario),

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
                return Ok(new TokenDto()
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
