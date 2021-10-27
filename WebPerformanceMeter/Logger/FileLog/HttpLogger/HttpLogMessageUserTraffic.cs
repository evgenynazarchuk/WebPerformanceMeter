namespace WebPerformanceMeter.Logger
{
    public class HttpLogMessageUserTraffic
    {
        public string? UserName { get; set; }

        public long Time { get; set; }

        public long SentBytes { get; set; }

        public long ReceivedBytes { get; set; }

        public HttpLogMessageUserTraffic(
            string? userName,
            long time,
            long sentBytes,
            long receivedBytes)
        {
            this.UserName = userName;
            this.Time = time;
            this.SentBytes = sentBytes;
            this.ReceivedBytes = receivedBytes;
        }
    }
}
