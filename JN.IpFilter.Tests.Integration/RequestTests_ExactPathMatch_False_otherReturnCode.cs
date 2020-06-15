using System.Net;
using System.Threading.Tasks;
using JN.IpFilter.Tests.Integration.HelperClasses;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace JN.IpFilter.Tests.Integration
{
    public class RequestTests_ExactPathMatch_False_otherReturnCode
    {
        private const string accessAllowedByIP_Path = "/Weather";
        private const string accessNotAllowedPath = "/Weather3";
        private const string pathNotInRulesStartingLikeOther = "/Weather4";

        private TestServer _apiServer;

        private readonly HttpStatusCode _otherDefaultResponseCode = HttpStatusCode.Unauthorized;


        [SetUp]
        public void Setup()
        {

            _apiServer = new TestServer(WebHost
                .CreateDefaultBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings_ExactPathMatch_False_otherReturnCode.json");
                })
                .UseStartup<FakeStartup>()
            );
        }


        [Test]
        public async Task Request_AccessAllowedByIP_OtherIp_returnsDefaultCode()
        {
            var response = await _apiServer.CreateRequest(accessAllowedByIP_Path)
                .AddHeader(Constants.HeaderFakeIpName, "1.2.4.6")
                .GetAsync();
            Assert.That(response.StatusCode == _otherDefaultResponseCode);
        }


        [Test]
        public async Task Request_accessNotAllowed_returnsDefaultCode()
        {
            var response = await _apiServer.CreateRequest(accessNotAllowedPath)
                .GetAsync();

            Assert.That(response.StatusCode == _otherDefaultResponseCode);
        }

  
        /// <summary>
        /// path is /weather4 but is following rule for /weather because ExactPathMatch = False
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Request_accessAllowed_pathNotInRulesStartingLikeOther_UserOtherRule_FromOtherIP_returnsDefaultCode()
        {
            var response = await _apiServer.CreateRequest(pathNotInRulesStartingLikeOther)
                .AddHeader(Constants.HeaderFakeIpName, "1.2.3.4")
                .GetAsync();

            Assert.That(response.StatusCode == _otherDefaultResponseCode);
        }
    }
}