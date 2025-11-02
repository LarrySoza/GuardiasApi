using App.WebApi.Models.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace App.WebApi.Controllers
{
    //[ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(Roles = "ADMIN")]
    [ApiController]
    [Route("api/[controller]")]
    public class JwtController : ControllerBase
    {
        private readonly ILogger<JwtController> _logger;
        private readonly IConfiguration _config;


        public JwtController(IConfiguration config, ILogger<JwtController> logger)
        {
            _config = config;
            _logger = logger;
        }

        /// <summary>
        /// Leer la configuracion JWT
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(JwtConfig), (int)HttpStatusCode.OK)]
        [HttpGet(Name = "LeerConfiguracionJwt")]
        public ActionResult LeerConfiguracionJwt()
        {
            try
            {
                var _jwtClass = new JwtConfigManager(_config);

                var _configJwt = _jwtClass.LoadConfig();

                return Ok(_configJwt);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Actualizar la configuracion JWT
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
        [HttpPut(Name = "GuardarJwtConfig")]
        public IActionResult GuardarJwtConfig([FromBody] JwtConfig config)
        {
            try
            {
                var _jwtClass = new JwtConfigManager(_config);

                _jwtClass.UpdateConfig(config);

                return Ok(new ResponseDto
                {
                    success = true,
                    message = "Configuración actualizada correctamente"
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
