namespace WebPerformanceMeter.Reports
{
    public class ChromiumLogMessage
    {
        public string? UserName { get; set; }

        public string? Url { get; set; }

        public string? Label { get; set; }

        public long StartTime { get; set; }

        public long EndTime { get; set; }
    }
}
