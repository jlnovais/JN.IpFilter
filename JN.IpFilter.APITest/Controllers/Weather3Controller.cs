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
    public class Weather3Controller : ControllerBase
    {
        private readonly ILogger<Weather3Controller> _logger;
        private readonly IWeatherService _service;

        public Weather3Controller(ILogger<Weather3Controller> logger, IWeatherService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            _logger.LogInformation("request received for Weather 3 API");

            return _service.GetWeather();
        }
    }
}
