using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace DFOLoginTool
{
    public class WebClientEx : WebClient
    {
        public WebClientEx(CookieContainer container)
        {
            this.container = container;
        }
        public WebClientEx()
        {

        }
        private readonly CookieContainer container = new CookieContainer();

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest r = base.GetWebRequest(address);
            var request = r as HttpWebRequest;
            if (request != null)
            {
                request.CookieContainer = container;
            }
            return r;
        }

        protected override WebResponse GetWebResponse(WebRequest request, IAsyncResult result)
        {
            WebResponse response = base.GetWebResponse(request, result);
            ReadCookies(response);
            return response;
        }

        protected override WebResponse GetWebResponse(WebRequest request)
        {
            WebResponse response = base.GetWebResponse(request);
            ReadCookies(response);
            return response;
        }

        private void ReadCookies(WebResponse r)
        {
            var response = r as HttpWebResponse;
            if (response != null)
            {
                CookieCollection cookies = new CookieCollection();// = response.Cookies;

                // Hilariously, Neople's ASP.NET webapp does not provide Set-Cookie in a format C# can autoparse, so response.cookies is empty. ugh microsoft
                for (int i = 0; i < response.Headers.Count; i++)
                {
                    string name = response.Headers.GetKey(i);
                    string value = response.Headers.Get(i);
                    if (name == "Set-Cookie")
                    {
                        Match match = Regex.Match(value, "(.+?)=(.+?);");
                        if (match.Captures.Count > 0)
                        {
                            cookies.Add(new Cookie(match.Groups[1].Value, match.Groups[2].Value, "/", "dfoneople.com"));
                        }
                    }
                }
                container.Add(cookies);
            }
        }
    }
}