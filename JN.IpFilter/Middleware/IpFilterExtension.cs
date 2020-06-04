using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;

namespace JN.IpFilter.Middleware
{
    public static class IpFilterExtension
    {
        public static IApplicationBuilder UseIpFilter(this IApplicationBuilder builder,
            IEnumerable<IpFilter> ipWhiteLists, bool logRequests = false, string applyOnlyToHttpMethod = "")
        {
            return builder.UseMiddleware<IpFilterMiddleware>(ipWhiteLists, logRequests, applyOnlyToHttpMethod);
            //return builder.UseMiddleware<IpFilterMiddleware>(applyOnlyToHttpMethod);
        }
    }
}