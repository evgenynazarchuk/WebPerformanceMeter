namespace WebPerformanceMeter.Logger.HttpClientLog
{
    public class HttpClientToolLogMessage
    {
        //public HttpClientToolLogMessage(
        //    string user,
        //    string requestMethod,
        //    string request,
        //    string requestLabel,
        //    int statusCode,
        //    long startSendRequestTime,
        //    long startWaitResponseTime,
        //    long startResponseTime,
        //    long endResponseTime,
        //    long sendBytes,
        //    int receiveBytes)
        //{
        //    this.User = user;
        //    this.RequestMethod = requestMethod;
        //    this.Request = request;
        //    this.RequestLabel = requestLabel;
        //    this.StatusCode = statusCode;
        //    this.StartSendRequestTime = startSendRequestTime;
        //    this.StartWaitResponseTime = startWaitResponseTime;
        //    this.StartResponseTime = startResponseTime;
        //    this.EndResponseTime = endResponseTime;
        //    this.SendBytes = sendBytes;
        //    this.ReceiveBytes = receiveBytes;
        //}

        public string? User { get; set; }

        public string? RequestMethod { get; set; }

        public string? Request { get; set; }

        public string? RequestLabel { get; set; }

        public int StatusCode { get; set; }

        public long StartSendRequestTime { get; set; }

        public long StartWaitResponseTime { get; set; }

        public long StartResponseTime { get; set; }

        public long EndResponseTime { get; set; }

        public long SendBytes { get; set; }

        public int ReceiveBytes { get; set; }
    }
}
