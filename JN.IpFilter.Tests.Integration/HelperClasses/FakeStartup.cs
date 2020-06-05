using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using JN.IpFilter.APITest;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JN.IpFilter.Tests.Integration.HelperClasses
{
    /// <summary>
    /// based on:
    /// https://stackoverflow.com/questions/49244283/set-dummy-ip-address-in-integration-test-with-asp-net-core-testserver
    /// </summary>
    public class FakeRemoteIpAddressMiddleware
    {
        private readonly RequestDelegate _next;
        private string _fakeIp = "1.1.1.1";
        public FakeRemoteIpAddressMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Headers.ContainsKey(Constants.HeaderFakeIpName))
                _fakeIp = httpContext.Request.Headers[Constants.HeaderFakeIpName];

            IPAddress fakeIpAddress = IPAddress.Parse(_fakeIp);

            httpContext.Connection.RemoteIpAddress = fakeIpAddress;

            await _next(httpContext);
        }
    }

    public class FakeStartup : Startup
    {
        public FakeStartup(IConfiguration configuration) : base(configuration)
        {
            var configuration2 = configuration;
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddApplicationPart(typeof(Startup).Assembly);

            base.ConfigureServices(services);
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<FakeRemoteIpAddressMiddleware>();
            base.Configure(app, env);
        }
    }
}
