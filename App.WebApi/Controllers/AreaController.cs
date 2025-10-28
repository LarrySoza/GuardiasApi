using App.WebApi.Entities;
using App.WebApi.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace App.WebApi.Controllers
{
    [Authorize(Roles = "Administrador")]
    [ApiController]
    [Route("admin/[controller]")]
    public class AreaController : ControllerBase
    {
        private readonly ILogger<AreaController> _logger;
        private readonly IConfiguration _config;

        public AreaController(IConfiguration config, ILogger<AreaController> logger)
        {
            _config = config;
            _logger = logger;
        }

        [ProducesResponseType(typeof(AreaId), (int)HttpStatusCode.OK)]
        [HttpPost(Name = "GuardarArea")]
        public async Task<IActionResult> GuardarArea([FromBody] AreaDto area)
        {
            try
            {
                var _areaClass = new AreaClass(_config);

                if (!(await _areaClass.GuardarAsync(area) is Guid areaId))
                {
                    throw new Exception("No se pudo guardar el área");
                }

                return Ok(new AreaId
                {
                    area_id = areaId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        [ProducesResponseType(typeof(List<Area>), (int)HttpStatusCode.OK)]
        [HttpGet(Name = "ListarAreas")]
        public async Task<IActionResult> ListarAreas([FromQuery] string? nombre)
        {
            try
            {
                var _areaClass = new AreaClass(_config);

                var _areas = string.IsNullOrWhiteSpace(nombre) ? await _areaClass.ListarAsync() : await _areaClass.BuscarAsync(nombre);

                return Ok(_areas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
        [HttpDelete(Name = "EliminarArea")]
        public async Task<IActionResult> Eliminar([FromQuery] Guid areaId)
        {
            try
            {
                var _areaClass = new AreaClass(_config);
                await _areaClass.EliminarAsync(areaId);
                return Ok(new ResponseDto
                {
                    success = true,
                    message = "Área eliminada correctamente"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
        [HttpPut("usuario", Name = "AsignarUsuarioArea")]
        public async Task<IActionResult> AsignarUsuarioArea([FromBody] UsuarioAreaDto usuarioArea)
        {
            try
            {
                var _areaClass = new AreaClass(_config);

                await _areaClass.AsignarUsuario(usuarioArea.area_id, usuarioArea.usuario_id);
                return Ok(new ResponseDto
                {
                    success = true,
                    message = "Usuario asignado correctamente al área"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        [ProducesResponseType(typeof(PaginaDatosModel<VwUsuarioPerfil>), (int)HttpStatusCode.OK)]
        [HttpGet("usuario/lista", Name = "ListarUsuarioArea")]
        public async Task<IActionResult> ListarUsuarioArea([FromQuery] Guid areaId,
                                                      [FromQuery] int page = 1,
                                                      [FromQuery] int pageSize = 20)
        {
            try
            {
                var _clsPasajero = new UsuarioClass(_config);
                var _result = await _clsPasajero.ListarPorAreaAsync(areaId, page, pageSize);
                return Ok(_result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

    }
}
