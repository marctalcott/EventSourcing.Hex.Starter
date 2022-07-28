using System.Threading.Tasks;
using APIC.ViewModels;
using Domain.ES.EventStore;
using Microsoft.AspNetCore.Mvc;

namespace Port.API.Commands.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdvertiserController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Add(AdvertiserViewModel vm)
        {
            var eventUserInfo = new EventUserInfo();
            return new OkResult();

            // return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            //     {
            //         Date = DateTime.Now.AddDays(index),
            //         TemperatureC = Random.Shared.Next(-20, 55),
            //         Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            //     })
            //     .ToArray();
        }
    }
}