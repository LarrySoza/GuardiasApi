using App.Application.Interfaces.Core;
using App.WebApi.Models.PanicAlert;
using App.WebApi.Models.Shared;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace App.WebApi.Controllers.Admin
{
    [Authorize(Roles = "ADMIN")]
    [ApiController]
    [Route("api/admin/[controller]")]
    public class PanicAlertAdjuntoController : ControllerBase
    {
        private readonly ILogger<PanicAlertAdjuntoController> _logger;
        private readonly IConfiguration _config;
        private readonly IPanicAlertAdjuntoRepository _repo;
        private readonly IMapper _mapper;

        public PanicAlertAdjuntoController(IConfiguration config, ILogger<PanicAlertAdjuntoController> logger, IPanicAlertAdjuntoRepository repo, IMapper mapper)
        {
            _config = config;
            _logger = logger;
            _repo = repo;
            _mapper = mapper;
        }

        /// <summary>
        /// Agrega un adjunto a una alerta de pánico.
        /// </summary>
        /// <param name="panicAlertId">Identificador de la alerta.</param>
        /// <param name="file">Archivo a subir (form-data).</param>
        /// <returns>201 Created con id del adjunto.</returns>
        [HttpPost("{panicAlertId:guid}", Name = "Admin_PanicAlertAdjunto_Crear")]
        [ProducesResponseType(typeof(GenericResponseIdDto<Guid>), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Crear(Guid panicAlertId, IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0) return BadRequest("Archivo inválido");

                using var stream = file.OpenReadStream();
                var id = await _repo.AddAsync(User.Id(), panicAlertId, stream, file.FileName);

                return CreatedAtRoute("Admin_PanicAlertAdjunto_ObtenerPorId", new { id = id }, new GenericResponseIdDto<Guid>(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Obtiene un adjunto por su id.
        /// </summary>
        [HttpGet("{id:guid}", Name = "Admin_PanicAlertAdjunto_ObtenerPorId")]
        [ProducesResponseType(typeof(PanicAlertAdjuntoDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> ObtenerPorId(Guid id)
        {
            try
            {
                var item = await _repo.GetByIdAsync(id);
                if (item == null) return NotFound();

                var dto = _mapper.Map<PanicAlertAdjuntoDto>(item);

                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Lista adjuntos de una alerta.
        /// </summary>
        [HttpGet("by-panic/{panicAlertId:guid}", Name = "Admin_PanicAlertAdjunto_ListByPanic")]
        [ProducesResponseType(typeof(List<PanicAlertAdjuntoDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll(Guid panicAlertId)
        {
            try
            {
                var items = await _repo.GetAllAsync(panicAlertId);
                var dtos = _mapper.Map<List<PanicAlertAdjuntoDto>>(items);

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Elimina (soft delete) un adjunto por su identificador.
        /// </summary>
        /// <param name="id">Identificador del adjunto.</param>
        [HttpDelete("{id:guid}", Name = "Admin_PanicAlertAdjunto_Eliminar")]
        [ProducesResponseType(typeof(GenericResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Eliminar(Guid id)
        {
            try
            {
                var existing = await _repo.GetByIdAsync(id);
                if (existing == null) return NotFound();

                await _repo.DeleteAsync(id);
                return Ok(new GenericResponseDto { success = true, message = "Adjunto eliminado correctamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
