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
            var _usuario = await _unitOfWork.Usuarios.GetByIdAsync(login.usuario);

            if (_usuario != null)
            {
                if (Crypto.VerifyHashedPassword(_usuario.clave_hash, login.clave))
                {
                    var _claims = new List<Claim>();
                    _claims.Add(new Claim(JwtClass.ClaimUsuarioId, _usuario.usuario_id.ToString()));
                    _claims.Add(new Claim(JwtClass.ClaimSecurity, _usuario.sello_seguridad.ToString()));

                    var _roles = await _usuarioClass.ListarRolesAsync(_usuario.usuario_id);

                    foreach (var _rol in _roles)
                    {
                        _claims.Add(new Claim(ClaimTypes.Role, _rol.nombre));
                    }

                    return new ClaimsIdentity(_claims.ToArray());
                }
            }

            return default;
        }
    }
}
