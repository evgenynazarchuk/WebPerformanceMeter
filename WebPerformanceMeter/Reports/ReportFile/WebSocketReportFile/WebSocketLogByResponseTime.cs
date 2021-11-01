namespace WebPerformanceMeter.Reports
{
    public class WebSocketLogByResponseTime
    {
        public WebSocketLogByResponseTime(
            string? userName,
            string? actionType,
            string? label,
            long endTime,
            long responseTime) 
        {
            this.UserName = userName;
            this.ActionType = actionType;
            this.Label = label;
            this.EndTime = endTime;
            this.ResponseTime = responseTime;
        }

        public string? UserName {  get; set; }

        public string? ActionType {  get; set; }

        public string? Label {  get; set; } 

        public long? EndTime { get; set; }

        public long? ResponseTime {  get; set; } 
    }
}
