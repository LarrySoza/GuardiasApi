using App.Application.Interfaces;
using App.WebApi.Configuration;
using App.WebApi.Models.Auth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace App.WebApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;

        public AuthService(IUnitOfWork unitOfWork, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _config = config;
        }

        public async Task<LoginResponseDto?> AuthenticateAsync(LoginRequestDto request)
        {
            ClaimsIdentity? claimsUser = default;

            var usuario = await _unitOfWork.Usuarios.GetByNameAsync(request.nombre_usuario);

            if (usuario != null)
            {
                if (Infrastructure.Crypto.VerifyHashedPassword(usuario.contrasena_hash!, request.contrasena))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(JwtConfigManager.ClaimUsuarioId, usuario.id.ToString()),
                        new Claim(JwtConfigManager.ClaimSecurity, usuario.sello_seguridad.ToString())
                    };

                    var roles = await _unitOfWork.UsuarioRoles.GetAllAsync(usuario.id);
                    foreach (var rol in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, rol.nombre!));
                    }

                    claimsUser = new ClaimsIdentity(claims.ToArray());
                }
            }

            if (claimsUser == null)
                return null;

            var jwtClass = new JwtConfigManager(_config);
            var configJwt = jwtClass.LoadConfig();

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configJwt.SecretKey));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var now = DateTime.UtcNow;
            var lifetimeSeconds = configJwt.LifetimeSeconds;
            var expires = TimeSpan.FromSeconds(lifetimeSeconds);
            var iat = new DateTimeOffset(now).ToUniversalTime().ToUnixTimeSeconds();

            var claimsStandard = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, request.nombre_usuario),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, iat.ToString(), ClaimValueTypes.Integer64)
            };

            foreach (var item in claimsUser.Claims)
            {
                claimsStandard.Add(item);
            }

            var exp = now.Add(expires);

            var tokenOptions = new JwtSecurityToken(
            issuer: configJwt.Issuer,
            audience: configJwt.Audience,
            claims: claimsStandard,
            expires: exp,
            signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return new LoginResponseDto
            {
                access_token = tokenString,
                token_type = "JWT",
                expires = exp
            };
        }
    }
}
