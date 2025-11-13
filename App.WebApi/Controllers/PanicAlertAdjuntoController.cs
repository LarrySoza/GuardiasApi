using App.Application.Interfaces;
using App.WebApi.Models.Shared;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace App.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PanicAlertAdjuntoController : ControllerBase
    {
        private readonly ILogger<PanicAlertAdjuntoController> _logger;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public PanicAlertAdjuntoController(IConfiguration config, ILogger<PanicAlertAdjuntoController> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _config = config;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Agrega un adjunto a una alerta de pánico.
        /// </summary>
        /// <param name="panicAlertId">Identificador de la alerta.</param>
        /// <param name="file">Archivo a subir (form-data).</param>
        /// <returns>201 Created con id del adjunto.</returns>
        [HttpPost("{panicAlertId:guid}", Name = "PanicAlertAdjunto_Crear")]
        [ProducesResponseType(typeof(GenericResponseIdDto<Guid>), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Crear(Guid panicAlertId, IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0) return BadRequest("Archivo inválido");

                var _panicAlert = await _unitOfWork.PanicAlerts.GetByIdAsync(panicAlertId);

                if (_panicAlert == null)
                {
                    return BadRequest("Alerta de pánico no encontrada");
                }

                var _sesionUsuario = await _unitOfWork.SesionUsuarios.GetByIdAsync(_panicAlert.sesion_usuario_id);

                if (_sesionUsuario!.usuario_id != User.Id())
                {
                    return Forbid();
                }

                using var stream = file.OpenReadStream();
                var id = await _unitOfWork.PanicAlertAdjuntos.AddAsync(User.Id(), panicAlertId, stream, file.FileName);

                return CreatedAtRoute("Admin_PanicAlertAdjunto_ObtenerPorId", new { id = id }, new GenericResponseIdDto<Guid>(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
