using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DFOLoginTool
{
    class WebClientEx : WebClient
    {
        private CookieContainer cookieContainer = new CookieContainer();


        public WebClientEx()
        {
            this.Headers.Add("User-Agent: Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; WOW64; Trident/7.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E; InfoPath.3)");
        }

        public CookieContainer CookieContainer
        {
            get
            {
                return this.cookieContainer;
            }
            set
            {
                this.cookieContainer = value;
            }
        }

        protected override WebRequest GetWebRequest(Uri uri)
        {
            WebRequest webRequest = base.GetWebRequest(uri);
            webRequest.Timeout = 5000;

            if (webRequest is HttpWebRequest)
            {
                var httpWebRequest = (HttpWebRequest)webRequest;
                httpWebRequest.CookieContainer = this.cookieContainer;
            }

            return webRequest;
        }
    }
}