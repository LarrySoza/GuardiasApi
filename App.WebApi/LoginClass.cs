using System.Security.Claims;

namespace App.WebApi.Infrastructure
{
    public class LoginClass
    {
        private readonly IConfiguration _config;

        public LoginClass(IConfiguration config)
        {
            _config = config;
        }

        public async Task<ClaimsIdentity?> GetIdentityAsync(Login login)
        {
            var _usuarioClass = new UsuarioClass(_config);

            var _usuario = await _usuarioClass.ConsultarPorNombreUsuarioAsync(login.usuario);

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
