namespace WebPerformanceMeter.Logger.HttpClientLog
{
    public class HttpClientToolLogMessageByteAnalytic
    {
        public long EndResponseTime { get; set; }

        public long Count { get; set; }

        public HttpClientToolLogMessageByteAnalytic(
            long endResponseTime,
            long count)
        {
            this.EndResponseTime = endResponseTime;
            this.Count = count;
        }
    }
}
