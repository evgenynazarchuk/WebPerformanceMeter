namespace WebPerformanceMeter.Logger
{
    public class WebSocketLogMessage
    {
        public string? UserName { get; set; }

        public string? Label { get; set; }

        public string? ActionType { get; set; }

        public long StartTime { get; set; }

        public long EndTime { get; set; }
    }
}
