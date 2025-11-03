using App.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.WebApi.Controllers.Admin
{
    [Authorize(Roles = "ADMIN")]
    [ApiController]
    [Route("api/admin/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly ILogger<ClienteController> _logger;
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;

        public ClienteController(IConfiguration config, ILogger<ClienteController> logger, IUnitOfWork unitOfWork)
        {
            _config = config;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
    }
}
