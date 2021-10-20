namespace WebPerformanceMeter.Logger
{
    public class HttpLogMessageByStartedRequest
    {
        public string? User { get; set; }

        public string? RequestMethod { get; set; }

        public string? Request { get; set; }

        public string? RequestLabel { get; set; }

        public int StatusCode { get; set; }

        public long StartRequestTime { get; set; }

        public long CountStartedRequest { get; set; }

        public HttpLogMessageByStartedRequest(
            string? user,
            string? httpMethod,
            string? request,
            string? requestLabel,
            int statusCode,
            long startRequestTime,
            long countStartedRequest
            )
        {
            this.User = user;
            this.RequestMethod = httpMethod;
            this.RequestLabel = requestLabel;
            this.Request = request;
            this.StatusCode = statusCode;
            this.StartRequestTime = startRequestTime;
            this.CountStartedRequest = countStartedRequest;
        }
    }
}
