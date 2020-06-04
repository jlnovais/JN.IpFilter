using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace JN.IpFilter.APITest.HelperClasses
{
    public static class SettingsHelper
    {
        public static List<Middleware.IpFilter> GetIpFilters(this IConfiguration configuration, string sectionName)
        {
            var config = new List<Middleware.IpFilter>();

            configuration.Bind(sectionName, config);

            return config;

        }

        public static bool GetIpFiltersLogRequests(this IConfiguration configuration, string sectionName)
        {
            bool logRequests = configuration.GetValue<bool>(sectionName);


            return logRequests;

        }
    }
}
