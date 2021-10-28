namespace WebPerformanceMeter.Reports
{
    public class WebSocketLogByEndTime : WebSocketLogByTime
    {
        public WebSocketLogByEndTime(
            string? userName,
            string? actionType,
            string? label,
            long time,
            long count)
            : base(userName, actionType, label, time, count) { }
    }
}
