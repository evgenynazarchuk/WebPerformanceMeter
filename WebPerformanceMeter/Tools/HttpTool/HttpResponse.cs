using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace WebPerformanceMeter.Tools.HttpTool
{
    public sealed class HttpResponse
    {
        public readonly int StatusCode;

        public readonly byte[] ContentAsBytes;

        public readonly string? Filename;

        public string ContentAsUtf8String => Encoding.UTF8.GetString(ContentAsBytes);

        public HttpResponse(
            int statusCode, 
            byte[] content,
            string? filename
            )
        {
            StatusCode = statusCode;
            ContentAsBytes = content;
            Filename = filename;
        }
    }
}
