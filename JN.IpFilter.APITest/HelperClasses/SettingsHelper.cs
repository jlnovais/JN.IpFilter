using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JN.IpFilter.Middleware;
using Microsoft.Extensions.Configuration;

namespace JN.IpFilter.APITest.HelperClasses
{
    public static class SettingsHelper
    {
        public static List<IpFilter> GetIpFilters(this IConfiguration configuration, string sectionName)
        {
            var config = new List<IpFilter>();

            configuration.Bind(sectionName, config);

            return config;
        }

        public static IpFilterMiddlewareOptions GetIpFilterOptions(this IConfiguration configuration, string sectionName)
        {
            var config = new IpFilterMiddlewareOptions();

            configuration.Bind(sectionName, config);

            return config;

        }

    }
}
