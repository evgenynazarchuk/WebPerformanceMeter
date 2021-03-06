namespace WebPerformanceMeter.Reports
{
    public class GrpcLogByTime
    {
        public GrpcLogByTime(
            string? userName,
            string? method,
            string? label,
            long time,
            long count)
        {
            this.UserName = userName;
            this.Method = method;
            this.Label = label;
            this.Time = time;
            this.Count = count;
        }

        public string? UserName { get; set; }

        public string? Method { get; set; }

        public string? Label { get; set; }

        public long Time { get; set; }

        public long Count { get; set; }
    }
}
