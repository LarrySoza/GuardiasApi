using App.Application.Interfaces;
using App.WebApi.Models.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace App.WebApi.Controllers
{
    //[ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(Roles = "Administrador")]
    [ApiController]
    [Route("admin/[controller]")]
    public class JwtController : ControllerBase
    {
        private readonly ILogger<JwtController> _logger;
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;

        public JwtController(IConfiguration config, ILogger<JwtController> logger, IUnitOfWork unitOfWork)
        {
            _config = config;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Leer la configuracion JWT
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(JwtConfig), (int)HttpStatusCode.OK)]
        [HttpGet(Name = "LeerConfiguracionTokenJwt")]
        public async Task<ActionResult> LeerConfiguracionJwt()
        {
            try
            {
                var _jwtClass = new JwtClass(_config);

                var _configJwt = await _jwtClass.LeerConfigAsync();

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
        [HttpPut(Name = "GuardarConfiguracionTokenJwt")]
        public async Task<IActionResult> GuardarJwtConfig([FromBody] JwtConfig config)
        {
            try
            {
                var _jwtClass = new JwtClass(_config);

                await _jwtClass.ActualizarConfigAsync(config);

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
