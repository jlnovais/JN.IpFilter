using NUnit.Framework;

namespace JN.IpFilter.Tests
{
    public class OtherTools
    {
        [TestCase(200)]
        [TestCase(301)]
        [TestCase(500)]
        [TestCase(401)]
        [TestCase(403)]
        public void IsValidHttpStatusCode_validCodes_returnsTrue(int code)
        {
            var actualResult = HelperClasses.IpFilterTools.IsValidHttpStatusCode(code);

            Assert.IsTrue(actualResult);
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(5000)]
        public void IsValidHttpStatusCode_invalidCodes_returnsFalse(int code)
        {
            var actualResult = HelperClasses.IpFilterTools.IsValidHttpStatusCode(code);

            Assert.IsFalse(actualResult);
        }
    }
}