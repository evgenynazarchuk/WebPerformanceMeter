namespace WebPerformanceMeter.Logger
{
    public class HttpLogMessageByteAnalytic
    {
        public long EndResponseTime { get; set; }

        public long Count { get; set; }

        public HttpLogMessageByteAnalytic(
            long endResponseTime,
            long count)
        {
            this.EndResponseTime = endResponseTime;
            this.Count = count;
        }
    }
}
