namespace WebPerformanceMeter.Reports
{
    public class GrpcLogByEndTime : GrpcLogByTime
    {
        public GrpcLogByEndTime(
            string? userName,
            string? method,
            string? label,
            long time,
            long count)
            : base(userName, method, label, time, count) { }
    }
}
