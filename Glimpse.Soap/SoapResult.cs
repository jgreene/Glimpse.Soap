using System;
using System.Collections.Generic;
using System.Web.Services.Protocols;
namespace Glimpse.Soap
{
    public class SoapResult
    {
        public string Duration { get; set; }
        public string Url { get; set; }
        public string Method { get; set; }
        public object RequestArgs { get; set; }
        public object ResponseResult { get; set; }
        public string RequestXml { get; set; }
        public string ResponseXml { get; set; }
        public string Stacktrace { get; set; }
    }
}
