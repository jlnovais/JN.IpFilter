using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using JN.IpFilter.HelperClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace JN.IpFilter.Middleware
{
    public class IpFilterMiddlewareOptions
    {
        public bool ExactPathMatch { get; set; }
        public bool LogRequests { get; set; }
        public string ApplyOnlyToHttpMethod { get; set; }
    }


    public class IpFilterMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<IpFilterMiddleware> _logger;
        private readonly IEnumerable<IpFilter> _ipLists;
        private readonly IpFilterMiddlewareOptions _options;

        public IpFilterMiddleware(
            RequestDelegate next,
            ILogger<IpFilterMiddleware> logger,
            IEnumerable<IpFilter> ipLists,
            IpFilterMiddlewareOptions options
        )
        {
            _ipLists = ipLists;
            _options = options;
            _logger = logger;

            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var applyFilter = true;

            if (!string.IsNullOrWhiteSpace(_options.ApplyOnlyToHttpMethod))
                if (context != null)
                    applyFilter = context.Request.Method == _options.ApplyOnlyToHttpMethod;

            applyFilter = applyFilter && _ipLists != null;

            if (applyFilter && context != null)
            {
                var remoteIp = context.Connection.RemoteIpAddress;
                var path = context.Request.Path.Value;

                if (_options.LogRequests)
                    _logger.LogInformation($"Request from Remote IP address: {remoteIp} to '{path}'");

                var resultValidation = IpFilterTools.ValidatePathAndIp(remoteIp, path, _ipLists.ToList(), _options.ExactPathMatch);

                var badIp = resultValidation.pathExist && !resultValidation.ipExists;

                if (badIp)
                {
                    if (_options.LogRequests)
                        _logger.LogInformation($"Forbidden request from Remote IP address: {remoteIp} to '{path}'");
                    context.Response.StatusCode = (int) HttpStatusCode.Unauthorized; //401
                    return;
                }
            }

            await _next.Invoke(context);
        }


    }
}