using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
 

namespace Port.API.Commands.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthCheckController : BaseController
    {
       // private readonly Func<ILogger<HealthCheckController>> _logger;

        private IConfiguration _configuration;

        public HealthCheckController(
            //Func<ILogger<HealthCheckController>> logger,
            IConfiguration configuration)
        {
            //_logger = logger;
            _configuration = configuration;
        }
        
        [HttpGet("")]
        public async Task<IActionResult> Healthcheck()
        {
            var retVal = $"{_configuration["AppName"]} at UTC: {DateTime.UtcNow.ToString()}";
            return Ok(retVal);
        }

        [Authorize]
        [HttpGet("secure")]
        public async Task<IActionResult> HealthcheckSecure()
        {
            var retVal = $"{_configuration["AppName"]} at UTC: {DateTime.UtcNow.ToString()}";
            return Ok(retVal);
        }
    }
}