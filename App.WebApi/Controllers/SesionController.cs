using App.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SesionController : ControllerBase
    {
        private readonly ILogger<SesionController> _logger;
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;

        public SesionController(IConfiguration config, ILogger<SesionController> logger, IUnitOfWork unitOfWork)
        {
            _config = config;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
    }
}
