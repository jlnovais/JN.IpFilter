using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JN.IpFilter.APITest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MethodsController : ControllerBase
    {

        private readonly ILogger<MethodsController> _logger;

        public MethodsController(ILogger<MethodsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            return "Get Method";
        }

        [HttpPost]
        public string Post([FromBody] string content)
        {
            return "Content received:" + content;
        }

    }
}
