namespace WebPerformanceMeter.Logger
{
    public class HttpLogMessageByCompletedRequest
    {
        public HttpLogMessageByCompletedRequest(
            string? userName,
            string? requestMethod,
            string? request,
            string? requestLabel,
            int statusCode,
            long endResponseTime,
            long completedRequests,
            double responseTime,
            double sentTime,
            double waitTime,
            double receiveTime,
            long sentBytes,
            long receivedBytes)
        {
            this.UserName = userName;
            this.RequestMethod = requestMethod;
            this.Request = request;
            this.RequestLabel = requestLabel;
            this.StatusCode = statusCode;
            this.EndResponseTime = endResponseTime;
            this.CompletedRequests = completedRequests;
            this.ResponseTime = responseTime;
            this.SentTime = sentTime;
            this.WaitTime = waitTime;
            this.ReceivedTime = receiveTime;
            this.SentBytes = sentBytes;
            this.ReceivedBytes = receivedBytes;
        }

        public string? UserName { get; set; }

        public string? RequestMethod { get; set; }

        public string? Request { get; set; }

        public string? RequestLabel { get; set; }

        public int StatusCode { get; set; }

        public long EndResponseTime { get; set; }

        public long CompletedRequests { get; set; }

        public double ResponseTime { get; set; }

        public double SentTime { get; set; }

        public double WaitTime { get; set; }

        public double ReceivedTime { get; set; }

        public long SentBytes {  get; set; }

        public long ReceivedBytes {  get; set; }
    }
}
