using System.Threading.Tasks;
using Application.Advertisers.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Port.API.Commands.Models;

namespace Port.API.Commands.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdvertiserController : BaseController
    {
        private readonly ILogger<AdvertiserController> _logger;
        private IMediator _mediator;

        public AdvertiserController(ILogger<AdvertiserController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddAdvertiser(AddAdvertiserViewModel model)
        {
            var eventUserInfo = base.GetUserInfo();
            var command = new AddAdvertiser(eventUserInfo, model.Id, model.Name);
            var result = await _mediator.Send(command);
            return Ok();
        }

    }
}