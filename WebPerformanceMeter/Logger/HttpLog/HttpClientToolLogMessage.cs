namespace WebPerformanceMeter.Logger.HttpClientLog
{
    public class HttpClientToolLogMessage
    {
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
