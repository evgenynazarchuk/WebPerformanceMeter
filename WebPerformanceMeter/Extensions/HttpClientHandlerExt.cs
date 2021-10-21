using System.Collections.Generic;
using System.Net.Http;
using System.Net;

namespace WebPerformanceMeter.Extensions
{
    public static class HttpClientHandlerExt
    {
        public static void SetDefaultCookie(this HttpClientHandler handler, IEnumerable<Cookie>? cookies)
        {
            if (cookies is not null && handler is not null)
            {
                CookieContainer cookieContainer = new();
                foreach (var cookie in cookies)
                {
                    cookieContainer.Add(cookie);
                }

                handler.CookieContainer = cookieContainer;
                handler.UseCookies = true;
            }
        }
    }
}
