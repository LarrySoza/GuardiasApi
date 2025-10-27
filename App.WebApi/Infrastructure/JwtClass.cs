using App.WebApi.Entities;
using System.Text.Json;

namespace App.WebApi.Infrastructure
{
    public class JwtClass
    {
        public const string ClaimSecurity = "stampSecurity";
        public const string ClaimUsuarioId = "usuarioId";

        private readonly IConfiguration _config;

        public JwtClass(IConfiguration config)
        {
            _config = config;
        }

        public JwtConfig LeerConfig()
        {
            var _configuracionClass = new ConfiguracionClass(_config);

            var _configJwt = _configuracionClass.Consultar("jwtConfig");

            if (_configJwt != null)
            {
                var _json = Util.Decrypt(_configJwt.valor ?? "", true);

                var _jwtConfig = JsonSerializer.Deserialize<JwtConfig>(_json);

                if (_jwtConfig != null)
                {
                    return _jwtConfig;
                }
            }

            //Retornamos una configuracion por defecto
            return new JwtConfig()
            {
                SecretKey = "n`h}G01K{I{#B[4S$_^m=ISiC:7wy[?1",//debe tener minimo 32 caracteres
                Issuer = "EduTrackJWT",
                Audience = "https://gaspersoft.com",
                LifetimeSeconds = 3600
            };
        }

        public async Task<JwtConfig> LeerConfigAsync()
        {
            var _configuracionClass = new ConfiguracionClass(_config);

            var _configJwt = await _configuracionClass.ConsultarAsync("jwtConfig");

            if (_configJwt != null)
            {
                var _json = Util.Decrypt(_configJwt.valor ?? "", true);

                var _jwtConfig = JsonSerializer.Deserialize<JwtConfig>(_json);

                if (_jwtConfig != null)
                {
                    return _jwtConfig;
                }
            }

            //Retornamos una configuracion por defecto
            return new JwtConfig()
            {
                SecretKey = "n`h}G01K{I{#B[4S$_^m=ISiC:7wy[?1",//debe tener minimo 32 caracteres
                Issuer = "EduTrackJWT",
                Audience = "https://gaspersoft.com",
                LifetimeSeconds = 3600
            };
        }

        public async Task ActualizarConfigAsync(JwtConfig config)
        {
            if (config == null)
            {
                throw new Exception("Configuracion no valida");
            }

            if (string.IsNullOrEmpty(config.Issuer))
            {
                throw new Exception("Issuer no valido");
            }

            if (string.IsNullOrEmpty(config.Audience))
            {
                throw new Exception("Audience no valido");
            }

            if (config.LifetimeSeconds < 300)
            {
                throw new Exception("Se permite minimo 300 segundos");
            }

            if (config.SecretKey?.Length < 32)
            {
                throw new Exception("SecretKey debe tener al menso 32 caracteres");
            }

            var _json = JsonSerializer.Serialize(config);

            var _configuracionClass = new ConfiguracionClass(_config);

            await _configuracionClass.GuardarAsync(new Configuracion()
            {
                tipo_configuracion_id = "jwtConfig",
                valor = Util.Encrypt(_json, true)
            });
        }
    }
}
