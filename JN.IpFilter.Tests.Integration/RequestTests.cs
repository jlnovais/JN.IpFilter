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

        /* API Config:

  "IpFilters": [
    {
      "Path": "/Weather",
      "IpList": "1.1.1.1;::1"
    },
    {
      "Path": "/Weather2",
      "IpList": "*"
    },
    {
      "Path": "/Weather3",
      "IpList": "2.2.2.2"
    }
  ],
        */

        private string accessAllowedByIP_Path = "/Weather";
        private string accessAllowedAllIPs_Path = "/Weather2";
        private string accessNotAllowedPath = "/Weather3";
        private string accessAllowedPathNotInRules = "/Weather4";

        private string accessAllowedPathNotExistsOnAPI = "/nonExisting";
        private string accessAllowedPathNotPatchingAnyRule = "/Other";

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
        public async Task Request_accessAllowedPathNotInRules_returnsOk()
        {
            var response = await _apiServer.CreateRequest(accessAllowedPathNotInRules)
                .GetAsync();

            Assert.That(response.StatusCode == HttpStatusCode.OK);
        }

        [Test]
        public async Task Request_accessAllowed_PathNotExisting_returnsNotFound()
        {
            var response = await _apiServer.CreateRequest(accessAllowedPathNotExistsOnAPI)
                .GetAsync();

            Assert.That(response.StatusCode == HttpStatusCode.NotFound);
        }

        [Test]
        public async Task Request_accessAllowed_PathExistingButNotMatchingAnyRule_returnsOk()
        {
            var response = await _apiServer.CreateRequest(accessAllowedPathNotPatchingAnyRule)
                .GetAsync();

            Assert.That(response.StatusCode == HttpStatusCode.OK);
        }

        //
    }
}