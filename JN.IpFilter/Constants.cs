using System;
using System.Net;

namespace JN.IpFilter
{
    public static class Constants
    {
        public static string AnyIp => "*";

        public static HttpStatusCode DefaultHttpStatusCode => HttpStatusCode.Forbidden;
    }
}
