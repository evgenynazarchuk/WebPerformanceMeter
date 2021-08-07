namespace WebPerformanceMeter.Logger
{
    public class LogMessageByteAnalytic
    {
        public long EndResponseTime { get; set; }

        public int Count { get; set; }

        public LogMessageByteAnalytic(
            long endResponseTime, 
            int count)
        {
            this.EndResponseTime = endResponseTime;
            this.Count = count;
        }
    }
}
