using System;

namespace WebPerformanceMeter.Reports
{
    public class GrpcReportFileSingleton
    {
        private static readonly Lazy<GrpcReportFile> lazy = new(() => new());

        public static GrpcReportFile GetInstance()
        {
            return lazy.Value;
        }
    }
}
