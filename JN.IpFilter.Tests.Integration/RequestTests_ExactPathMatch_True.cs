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
    public class RequestTests_ExactPathMatch_True
    {
        private string pathNotInRulesStartingLikeOther = "/Weather4";

        private TestServer _apiServer;

        [SetUp]
        public void Setup()
        {

            _apiServer = new TestServer(WebHost
                .CreateDefaultBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings_ExactPathMatch_True.json");
                })
                .UseStartup<FakeStartup>()
            );
        }

        /// <summary>
        /// path is /weather4 but is NOT following rule for /weather because ExactPathMatch = True
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Request_accessAllowed_pathNotInRulesStartingLikeOther_FromOtherIP_returnsOk()
        {
            var response = await _apiServer.CreateRequest(pathNotInRulesStartingLikeOther)
                .AddHeader(Constants.HeaderFakeIpName, "1.2.3.4")
                .GetAsync();

            Assert.That(response.StatusCode == HttpStatusCode.OK);
        }
    }
}