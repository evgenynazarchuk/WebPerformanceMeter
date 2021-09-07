namespace WebPerformanceMeter.Logger
{
    public class HttpClientLogMessage
    {
        public HttpClientLogMessage(
            string user,
            string protocolType,
            string request,
            string requestLabel,
            int statusCode,
            long startSendRequestTime,
            long startWaitResponseTime,
            long startResponseTime,
            long endResponseTime,
            int sendBytes,
            int receiveBytes)
        {
            this.User = user;
            this.ProtocolType = protocolType;
            this.Request = request;
            this.RequestLabel = requestLabel;
            this.StatusCode = statusCode;
            this.StartSendRequestTime = startSendRequestTime;
            this.StartWaitResponseTime = startWaitResponseTime;
            this.StartResponseTime = startResponseTime;
            this.EndResponseTime = endResponseTime;
            this.SendBytes = sendBytes;
            this.ReceiveBytes = receiveBytes;
        }

        public string User { get; set; }

        public string ProtocolType { get; set; }

        public string Request { get; set; }

        public string RequestLabel { get; set; }

        public int StatusCode { get; set; }

        public long StartSendRequestTime { get; set; }

        public long StartWaitResponseTime { get; set; }

        public long StartResponseTime { get; set; }

        public long EndResponseTime { get; set; }

        public int SendBytes { get; set; }

        public int ReceiveBytes { get; set; }
    }
}
