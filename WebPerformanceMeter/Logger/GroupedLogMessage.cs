namespace WebPerformanceMeter.Logger
{
    public class GroupedLogMessage
    {
        public string User { get; set; }

        public string Request { get; set; }

        public string RequestLabel { get; set; }

        public int StatusCode { get; set; }

        public long EndResponse { get; set; }

        public long CompletedRequest { get; set; }

        public double ResponseTime { get; set; }

        public double SentTime { get; set; }

        public double WaitTime { get; set; }

        public double ReceivedTime { get; set; }

        public GroupedLogMessage(
            string user,
            string request,
            string requestLabel,
            int statusCode,
            long endResponse,
            long completedRequest,
            double responseTime,
            double sentTime,
            double waitTime,
            double receiveTime)
        {
            User = user;
            Request = request;
            RequestLabel = requestLabel;
            StatusCode = statusCode;
            EndResponse = endResponse;
            CompletedRequest = completedRequest;
            ResponseTime = responseTime;
            SentTime = sentTime;
            WaitTime = waitTime;
            ReceivedTime = receiveTime;
        }
    }
}
