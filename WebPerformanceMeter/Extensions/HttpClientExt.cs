using System.Collections.Generic;
using System.Net.Http;

namespace WebPerformanceMeter.Extensions
{
    public static class HttpClientExt
    {
        public static void SetDefaultHeader(this HttpClient client, IDictionary<string, string>? headers)
        {
            if (headers is not null)
            {
                foreach (var (headerName, headerValue) in headers)
                {
                    client.DefaultRequestHeaders.Add(headerName, headerValue);
                }
            }
        }
    }
}
