namespace WebPerformanceMeter.Report
{
    public class GrpcLogMessage
    {
        public string? UserName { get; set; }

        public string? Method { get; set; }

        public string? Label { get; set; }

        public long StartTime { get; set; }

        public long EndTime { get; set; }
    }
}
