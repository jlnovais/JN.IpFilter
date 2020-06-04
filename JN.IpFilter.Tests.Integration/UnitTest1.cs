using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using JN.IpFilter.APITest;
using JN.IpFilter.Tests.Integration.HelperClasses;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace JN.IpFilter.Tests.Integration
{
    public class Tests
    {
        private TestServer _apiServer;

        [SetUp]
        public void Setup()
        {
            _apiServer = new TestServer(WebHost
                .CreateDefaultBuilder()
                .UseStartup<StartupStub>());
        }



        [Test]
        public async Task Test1()
        {
            var path = "/Weather";

            var response = await _apiServer.CreateRequest(path)
                .GetAsync();

            Assert.That(response.StatusCode == HttpStatusCode.OK);
        }
    }
}