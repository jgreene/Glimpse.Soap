using System;
using System.Collections.Generic;
using System.Web.Services.Protocols;
namespace Glimpse.Soap
{
    public class SoapResult
    {
        public TimeSpan Time
        {
            get;
            set;
        }
        public string Url
        {
            get;
            set;
        }
        public string Method
        {
            get;
            set;
        }
        public string Request
        {
            get;
            set;
        }
        public string Response
        {
            get;
            set;
        }
    }
}
