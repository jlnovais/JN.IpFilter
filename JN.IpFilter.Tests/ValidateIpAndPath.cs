using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using NUnit.Framework;

namespace JN.IpFilter.Tests
{
    public class ValidateIpAndPathTests
    {
        private readonly List<IpFilter> _filters = new List<IpFilter>
        {
            new IpFilter
            {
                Path= "/Path",
                IpList= "1.1.1.1;::1"
            },
            new IpFilter
            {
                Path= "/Path2",
                IpList= "*"
            },
            new IpFilter
            {
                Path = "/Path3",
                IpList = "2.2.2.2"
            }
        };


        [SetUp]
        public void Setup()
        {
        }

        [TestCase("/Path2", "1.1.1.1")]
        [TestCase("/Path2", "0.0.0.0")]
        [TestCase("/Path2", "::1")]
        public void ValidatePathAndIp_RuleAnyIP_returnsExpectedResults(string path, string ip)
        {
            var ipAddress = IPAddress.Parse(ip);

            var actualResult = HelperClasses.IpFilterTools.ValidatePathAndIp(ipAddress, path, _filters, false);

            Assert.IsTrue(actualResult.ipExists);
            Assert.IsTrue(actualResult.pathExist);
        }


        [TestCase( "/nonExisting", "127.0.0.1",false, false)]
        [TestCase("/nonExisting","1.1.1.1",  false, false)]
        [TestCase("/Path","127.0.0.1",  true, false)]
        [TestCase("/Path", "1.1.1.1", true, true)]
        [TestCase("/Path2", "1.1.1.1", true, true)]
        [TestCase("/Path_StartsWithOtherPath", "1.1.1.1", true, true)]
        [TestCase("/Path2_StartsWithOtherPath", "1.1.1.1", true, true)]
        public void ValidatePathAndIp_NoPathExactMatch_returnsExpectedResults(string path,string ip, bool expectedPathExist, bool expectedIpExists)
        {
            var ipAddress = IPAddress.Parse(ip);

            var actualResult = HelperClasses.IpFilterTools.ValidatePathAndIp(ipAddress, path, _filters, false);

            Assert.That(actualResult.ipExists == expectedIpExists);
            Assert.That(actualResult.pathExist == expectedPathExist);
        }

        [TestCase("/nonExisting", "127.0.0.1", false, false)]
        [TestCase("/nonExisting", "1.1.1.1", false, false)]
        [TestCase("/Path", "127.0.0.1", true, false)]
        [TestCase("/Path", "1.1.1.1", true, true)]
        [TestCase("/Path2", "1.1.1.1", true, true)]
        [TestCase("/Path_StartsWithOtherPath", "1.1.1.1", false, false)]
        [TestCase("/Path2_StartsWithOtherPath", "1.1.1.1", false, false)]
        public void ValidatePathAndIp_PathExactMatch_returnsExpectedResults(string path, string ip, bool expectedPathExist, bool expectedIpExists)
        {
            var ipAddress = IPAddress.Parse(ip);

            var actualResult = HelperClasses.IpFilterTools.ValidatePathAndIp(ipAddress, path, _filters, true);

            Assert.That(actualResult.ipExists == expectedIpExists);
            Assert.That(actualResult.pathExist == expectedPathExist);
        }

    }



}
