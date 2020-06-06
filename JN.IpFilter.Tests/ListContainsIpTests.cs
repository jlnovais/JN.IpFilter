using System.Net;
using NUnit.Framework;

namespace JN.IpFilter.Tests
{
    public class ListContainsIpTests
    {
 
        [Test]
        public void ListContainsIp_validIpSingle_returnsTrue()
        {
            var actualResult = HelperClasses.IpFilterTools.ListContainsIp("127.0.0.1", IPAddress.Parse("127.0.0.1"));

            Assert.IsTrue(actualResult);
        }

        [Test]
        public void ListContainsIp_validIpList_returnsTrue()
        {
            var actualResult = HelperClasses.IpFilterTools.ListContainsIp("127.0.0.1;192.168.0.1;::1", IPAddress.Parse("127.0.0.1"));

            Assert.IsTrue(actualResult);
        }

        [Test]
        public void ListContainsIp_validIpList_IPv6_returnsTrue()
        {
            var actualResult = HelperClasses.IpFilterTools.ListContainsIp("127.0.0.1;192.168.0.1;::1", IPAddress.Parse("::1"));

            Assert.IsTrue(actualResult);
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        [TestCase(";")]
        public void ListContainsIp_invalidIPList_returnsFalse(string ipList)
        {
            var actualResult = HelperClasses.IpFilterTools.ListContainsIp(ipList, IPAddress.Parse("127.0.0.1"));

            Assert.IsFalse(actualResult);
        }
    }
}