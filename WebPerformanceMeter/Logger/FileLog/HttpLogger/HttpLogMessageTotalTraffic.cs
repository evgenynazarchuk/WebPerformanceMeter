namespace WebPerformanceMeter.Logger
{
    public class HttpLogMessageTotalTraffic
    {
        public long Time { get; set; }

        public long SentBytes { get; set; }

        public long ReceivedBytes { get; set; }

        public HttpLogMessageTotalTraffic(
            long time,
            long sentBytes,
            long receivedBytes)
        {
            this.Time = time;
            this.SentBytes = sentBytes;
            this.ReceivedBytes = receivedBytes;
        }
    }
}
