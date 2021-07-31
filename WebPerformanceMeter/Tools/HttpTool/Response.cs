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
    public class Response
    {
        public readonly HttpStatusCode StatusCode;

        public readonly byte[] ContentAsBytes;

        public string ContentAsUtf8String => Encoding.UTF8.GetString(ContentAsBytes);

        public readonly HttpContentHeaders ContentHeaders;

        public Response(
            HttpStatusCode statusCode, 
            byte[] content,
            HttpContentHeaders contentHeaders
            )
        {
            StatusCode = statusCode;
            ContentAsBytes = content;
            ContentHeaders = contentHeaders;
        }
    }
}
