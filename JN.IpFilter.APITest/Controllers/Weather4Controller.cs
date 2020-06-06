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
    public class Weather4Controller : ControllerBase
    {
        private readonly ILogger<Weather4Controller> _logger;
        private readonly IWeatherService _service;

        public Weather4Controller(ILogger<Weather4Controller> logger, IWeatherService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            _logger.LogInformation("request received for Weather 4 API");

            return _service.GetWeather();
        }
    }
}
