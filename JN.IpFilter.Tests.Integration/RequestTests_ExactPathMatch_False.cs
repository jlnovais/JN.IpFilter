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
    public class RequestTests_ExactPathMatch_False
    {
        private string accessAllowedByIP_Path = "/Weather";
        private string accessAllowedAllIPs_Path = "/Weather2";
        private string accessNotAllowedPath = "/Weather3";
        private string pathNotInRulesStartingLikeOther = "/Weather4";
        private string pathNotInRules_Not_StartingLikeOther = "/SomeOther";

        private string accessAllowedPathNotExistsOnAPI = "/nonExisting";
        private string accessAllowedPathNotPatchingAnyRule = "/Other";

        private TestServer _apiServer;


        [SetUp]
        public void Setup()
        {

            _apiServer = new TestServer(WebHost
                .CreateDefaultBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings_ExactPathMatch_False.json");
                })
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
        public async Task Request_AccessAllowedByIP_OtherIp_returnsUnauthorized()
        {
            var response = await _apiServer.CreateRequest(accessAllowedByIP_Path)
                .AddHeader(Constants.HeaderFakeIpName, "1.2.4.6")
                .GetAsync();
            Assert.That(response.StatusCode == HttpStatusCode.Unauthorized);
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

        /// <summary>
        /// path is /weather4 but is following rule for /weather because ExactPathMatch = False
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Request_accessAllowed_pathNotInRulesStartingLikeOther_UserOtherRule_returnsOk()
        {
            var response = await _apiServer.CreateRequest(pathNotInRulesStartingLikeOther)
                .GetAsync();

            Assert.That(response.StatusCode == HttpStatusCode.OK);
        }


        /// <summary>
        /// path is /weather4 but is following rule for /weather because ExactPathMatch = False
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Request_accessAllowed_pathNotInRulesStartingLikeOther_UserOtherRule_FromOtherIP_returnsUnauthorized()
        {
            var response = await _apiServer.CreateRequest(pathNotInRulesStartingLikeOther)
                .AddHeader(Constants.HeaderFakeIpName, "1.2.3.4")
                .GetAsync();

            Assert.That(response.StatusCode == HttpStatusCode.Unauthorized);
        }


        [Test]
        public async Task Request_accessAllowed_pathNotInRules_NOT_StartingLikeOther_returnsOk()
        {
            var response = await _apiServer.CreateRequest(pathNotInRules_Not_StartingLikeOther)
                .GetAsync();

            Assert.That(response.StatusCode == HttpStatusCode.OK);
        }

        [Test]
        public async Task Request_accessAllowed_pathNotInRules_NOT_StartingLikeOther_From_OtherIP_returnsOk()
        {
            var response = await _apiServer.CreateRequest(pathNotInRules_Not_StartingLikeOther)
                .AddHeader(Constants.HeaderFakeIpName, "1.2.3.4")
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