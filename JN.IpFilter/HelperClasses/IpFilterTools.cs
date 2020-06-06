using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("JN.IpFilter.Tests")]
namespace JN.IpFilter.HelperClasses
{
    internal static class IpFilterTools
    {
        public static (bool pathExist, bool ipExists) ValidatePathAndIp(IPAddress remoteIp, string path,
            List<IpFilter> ipFilters, bool exactPathMatch)
        {
            var filtersWithPath = ipFilters.Where(x => path.Equals(x.Path, StringComparison.InvariantCultureIgnoreCase))
                .ToList();

            if (!exactPathMatch && !filtersWithPath.Any())
                filtersWithPath = ipFilters
                    .Where(x => path.StartsWith(x.Path, StringComparison.InvariantCultureIgnoreCase)).ToList();

            if (!filtersWithPath.Any())
                return (false, false);

            var res = filtersWithPath.Any(x => ListContainsIp(x.IpList, remoteIp));

            return (true, res);
        }

        public static bool ListContainsIp(string ipList, IPAddress remoteIp)
        {
            if (string.IsNullOrWhiteSpace(ipList))
                return false;

            string[] ips = ipList.Split(';');

            if (ips.Contains(Constants.AnyIp))
                return true;

            var newIpList = ips.Where(x => !string.IsNullOrWhiteSpace(x));

            var bytes = remoteIp.GetAddressBytes();

            return newIpList.Select(address => IPAddress.Parse(address))
                .Any(testIp => testIp.GetAddressBytes().SequenceEqual(bytes));
        }
    }
}
