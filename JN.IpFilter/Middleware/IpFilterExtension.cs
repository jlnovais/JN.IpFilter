using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;

namespace JN.IpFilter.Middleware
{
    public static class IpFilterExtension
    {
        public static IApplicationBuilder UseIpFilter(this IApplicationBuilder builder,
            IEnumerable<IpFilter> ipWhiteLists, IpFilterMiddlewareOptions options)
        {
            return builder.UseMiddleware<IpFilterMiddleware>(ipWhiteLists, options);
        }

    }
}