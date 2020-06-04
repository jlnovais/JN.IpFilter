using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using JN.IpFilter.APITest;
using JN.IpFilter.Tests.Integration.HelperClasses;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace JN.IpFilter.Tests.Integration
{
    public class RequestTests
    {

        private string accessAllowedByIP_Path = "/Weather";
        private string accessAllowedAllIPs_Path = "/Weather2";
        private string accessNotAllowedPath = "/Weather3";
        private string accessAllowed_IP_Not_Listed_Path = "/Weather4";

        private TestServer _apiServer;


        [SetUp]
        public void Setup()
        {
            _apiServer = new TestServer(WebHost
                .CreateDefaultBuilder()
                .UseStartup<FakeStartup>()
            );
        }


        [Test]
        public async Task Request_AccessAllowedByIP_returnsOK()
        {
            var response = await _apiServer.CreateRequest(accessAllowedByIP_Path)
                .GetAsync();
            Assert.That(response.StatusCode == HttpStatusCode.OK);
        }


        [Test]
        public async Task Request_accessAllowedAllIPs_returnsOK()
        {
            var response = await _apiServer.CreateRequest(accessAllowedAllIPs_Path)
                .GetAsync();

            Assert.That(response.StatusCode == HttpStatusCode.OK);
        }

        [Test]
        public async Task Request_accessNotAllowed_returnsUnauthorized()
        {
            var response = await _apiServer.CreateRequest(accessNotAllowedPath)
                .GetAsync();

            Assert.That(response.StatusCode == HttpStatusCode.Unauthorized);
        }

        [Test]
        public async Task Request_accessAllowed_IP_not_Listed_returnsUnauthorized()
        {
            var response = await _apiServer.CreateRequest(accessAllowed_IP_Not_Listed_Path)
                .GetAsync();

            Assert.That(response.StatusCode == HttpStatusCode.OK);
        }
    }
}