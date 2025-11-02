using App.Application.Interfaces;
using App.Infrastructure;
using App.WebApi.Models.Usuario;
using System.Security.Claims;

namespace App.WebApi
{
    public class LoginClass
    {
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;

        public LoginClass(IConfiguration config, IUnitOfWork unitOfWork)
        {
            _config = config;
            _unitOfWork = unitOfWork;
        }

        public async Task<ClaimsIdentity?> GetIdentityAsync(LoginDto login)
        {
            // Buscar usuario por nombre (LoginDto.usuario puede ser nombre o email según tu lógica)
            var usuario = await _unitOfWork.Usuarios.GetByNameAsync(login.usuario);

            if (usuario != null)
            {
                // verificar contraseña usando la utilidad Crypto
                if (Crypto.VerifyHashedPassword(usuario.contrasena_hash, login.clave))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(JwtClass.ClaimUsuarioId, usuario.id.ToString()),
                        new Claim(JwtClass.ClaimSecurity, usuario.sello_seguridad.ToString())
                    };

                    // Si necesitas roles, obténlos a través de un repo/servicio adecuado (por ejemplo _unitOfWork.UsuarioRoles)

                    return new ClaimsIdentity(claims.ToArray());
                }
            }

            return default;
        }
    }
}
