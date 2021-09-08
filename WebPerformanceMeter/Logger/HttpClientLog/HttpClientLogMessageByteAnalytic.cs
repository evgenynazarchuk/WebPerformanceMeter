namespace WebPerformanceMeter.Logger.HttpClientLog
{
    public class HttpClientLogMessageByteAnalytic
    {
        public long EndResponseTime { get; set; }

        public long Count { get; set; }

        public HttpClientLogMessageByteAnalytic(
            long endResponseTime,
            long count)
        {
            this.EndResponseTime = endResponseTime;
            this.Count = count;
        }
    }
}
