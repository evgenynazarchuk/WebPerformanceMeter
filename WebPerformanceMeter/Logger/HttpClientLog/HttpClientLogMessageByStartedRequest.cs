using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPerformanceMeter.Logger.HttpClientLog
{
    public class HttpClientLogMessageByStartedRequest
    {
        public string User { get; set; }

        public string RequestMethod { get; set; }

        public string Request { get; set; }

        public string RequestLabel { get; set; }

        public int StatusCode { get; set; }

        public long StartRequestTime { get; set; }

        public long CountStartedRequest { get; set; }

        public HttpClientLogMessageByStartedRequest(
            string user,
            string requestMethod,
            string request,
            string requestLabel,
            int statusCode,
            long startRequestTime,
            long countStartedRequest
            )
        {
            this.User = user;
            this.RequestMethod= requestMethod;
            this.RequestLabel = requestLabel;
            this.Request = request;
            this.StatusCode = statusCode;
            this.StartRequestTime = startRequestTime;
            this.CountStartedRequest = countStartedRequest;
        }
    }
}
