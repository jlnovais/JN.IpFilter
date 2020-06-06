using System.Collections.Generic;
using JN.IpFilter.APITest.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JN.IpFilter.APITest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SomeOtherController : ControllerBase
    {
        private readonly ILogger<SomeOtherController> _logger;
        private readonly IWeatherService _service;

        public SomeOtherController(ILogger<SomeOtherController> logger, IWeatherService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            _logger.LogInformation("request received for SomeOther API");

            return _service.GetWeather();
        }
    }
}
