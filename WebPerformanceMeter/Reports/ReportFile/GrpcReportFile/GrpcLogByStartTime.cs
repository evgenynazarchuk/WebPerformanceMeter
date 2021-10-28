namespace WebPerformanceMeter.Reports
{
    public class GrpcLogByStartTime : GrpcLogByTime
    {
        public GrpcLogByStartTime(
            string? userName,
            string? method,
            string? label,
            long time,
            long count)
            : base(userName, method, label, time, count) { }
    }
}
