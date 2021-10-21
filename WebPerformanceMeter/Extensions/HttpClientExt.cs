using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
