﻿namespace JN.IpFilter.Middleware
{
    public class IpFilterMiddlewareOptions
    {
        public bool ExactPathMatch { get; set; }
        public bool LogRequests { get; set; }
        public string ApplyOnlyToHttpMethod { get; set; }
        public int ResponseHttpStatusCode { get; set; }
        public string ResponseContentType { get; set; }
        public string ResponseContent { get; set; }
    }
}