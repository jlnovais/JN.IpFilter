using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JN.IpFilter.Tests.Integration.HelperClasses;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace JN.IpFilter.Tests.Integration
{
    public class RequestTests_POST_Method
    {

        private const string path = "/Methods";
        private TestServer _apiServer;

        private readonly HttpStatusCode _defaultHttpStatusCode = JN.IpFilter.Constants.DefaultHttpStatusCode;

        [SetUp]
        public void Setup()
        {

            _apiServer = new TestServer(WebHost
                .CreateDefaultBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings_POST_method.json");
                })
                .UseStartup<FakeStartup>()
            );
        }

        /// <summary>
        /// not validating IP
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Request_GetMethod_IpInRule_returnsOk()
        {
            var response = await _apiServer.CreateRequest(path)
                //.AddHeader(Constants.HeaderFakeIpName, "1.2.3.4")
                .GetAsync();

            Assert.That(response.StatusCode == HttpStatusCode.OK);
        }

        /// <summary>
        /// not validating IP
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Request_GetMethod_IpNotInRule_returnsOk()
        {
            var response = await _apiServer.CreateRequest(path)
                .AddHeader(Constants.HeaderFakeIpName, "1.2.3.4")
                .GetAsync();

            Assert.That(response.StatusCode == HttpStatusCode.OK);
        }

        /// <summary>
        /// validating IP
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Request_PostMethod_IpNotInRule_returnsDefaultCode()
        {
            var content = GetContent();

            var response = await _apiServer.CreateRequest(path)
                .And(x => x.Content = content)
                .AddHeader("Content-Type", "application/json")
                .AddHeader(Constants.HeaderFakeIpName, "1.2.3.4")
                .PostAsync();

            Assert.That(response.StatusCode == _defaultHttpStatusCode);
        }

        [Test]
        public async Task Request_PostMethod_IpInRule_returnsOk()
        {
            var content = GetContent();

            var response = await _apiServer.CreateRequest(path)
                .And(x => x.Content = content)
                .AddHeader("Content-Type", "application/json")
                .PostAsync();

            Assert.That(response.StatusCode == HttpStatusCode.OK);
        }

        public static StringContent GetContent()
        {
            return new StringContent("\"any text\"",
                Encoding.UTF8,
                "application/json");
        }

        //
    }
}