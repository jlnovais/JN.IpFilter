using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JN.IpFilter.APITest.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JN.IpFilter.APITest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OtherController : ControllerBase
    {
        private readonly ILogger<OtherController> _logger;
        private readonly IWeatherService _service;

        public OtherController(ILogger<OtherController> logger, IWeatherService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            _logger.LogInformation("request received for Other API");

            return _service.GetWeather();
        }
    }
}
