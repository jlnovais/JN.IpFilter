using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using JN.IpFilter.HelperClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace JN.IpFilter.Middleware
{
    public class IpFilterMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<IpFilterMiddleware> _logger;
        private readonly IEnumerable<IpFilter> _ipLists;
        private readonly string _applyOnlyToHttpMethod;
        private readonly bool _logRequests;

        public IpFilterMiddleware(
            RequestDelegate next,
            ILogger<IpFilterMiddleware> logger,
            IEnumerable<IpFilter> ipLists,
            bool logRequests,
            string applyOnlyToHttpMethod
        )
        {
            _ipLists = ipLists;
            _applyOnlyToHttpMethod = applyOnlyToHttpMethod;
            _logRequests = logRequests;
            _logger = logger;

            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var applyFilter = true;
            if (!string.IsNullOrWhiteSpace(_applyOnlyToHttpMethod))
                if (context != null)
                    applyFilter = context.Request.Method == _applyOnlyToHttpMethod;

            applyFilter = applyFilter && _ipLists != null;


            if (applyFilter)
            {
                var remoteIp = context.Connection.RemoteIpAddress;
                var path = context.Request.Path.Value;

                if (_logRequests)
                    _logger.LogInformation($"Request from Remote IP address: {remoteIp} to '{path}'");

                var resultValidation = IpFilterTools.ValidateIpAndPath(remoteIp, path, _ipLists.ToList());

                var badIp = resultValidation.pathExist && !resultValidation.ipExists;

                if (badIp)
                {
                    if (_logRequests)
                        _logger.LogInformation($"Forbidden request from Remote IP address: {remoteIp} to '{path}'");
                    context.Response.StatusCode = (int) HttpStatusCode.Unauthorized; //401
                    return;
                }
            }

            await _next.Invoke(context);
        }


    }
}