using App.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.WebApi.Controllers.Admin
{
    [Authorize(Roles = "ADMIN")]
    [ApiController]
    [Route("api/admin/[controller]")]
    public class UnidadController : ControllerBase
    {
        private readonly ILogger<UnidadController> _logger;
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;

        public UnidadController(IConfiguration config, ILogger<UnidadController> logger, IUnitOfWork unitOfWork)
        {
            _config = config;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
    }
}
