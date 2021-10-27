using System;

namespace WebPerformanceMeter.Reports
{
    public static class HttpReportFileSingleton
    {
        private static readonly Lazy<HttpReportFile> lazy = new(() => new());

        public static HttpReportFile GetInstance()
        {
            return lazy.Value;
        }
    }
}
