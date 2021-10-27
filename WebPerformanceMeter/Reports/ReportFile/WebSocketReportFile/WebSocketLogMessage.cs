namespace WebPerformanceMeter.Reports
{
    public class WebSocketLogMessage
    {
        public WebSocketLogMessage(
            string? userName,
            string? label,
            string? actionType,
            long startTime,
            long endTime)
        { 
            this.UserName = userName;
            this.Label = label;
            this.ActionType = actionType;
            this.StartTime = startTime;
            this.EndTime = endTime;
        }

        public string? UserName { get; set; }

        public string? Label { get; set; }

        public string? ActionType { get; set; }

        public long StartTime { get; set; }

        public long EndTime { get; set; }
    }
}
