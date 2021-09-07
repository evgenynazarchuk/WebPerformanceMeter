namespace WebPerformanceMeter.Logger
{
    public class HttpClientLogMessageByteAnalytic
    {
        public long EndResponseTime { get; set; }

        public int Count { get; set; }

        public HttpClientLogMessageByteAnalytic(
            long endResponseTime,
            int count)
        {
            this.EndResponseTime = endResponseTime;
            this.Count = count;
        }
    }
}
