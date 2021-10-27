using System;

namespace WebPerformanceMeter.Reports
{
    public static class ChromiumReportFileSingleton
    {
        private static readonly Lazy<ChromiumReportFile> lazy = new(() => new());

        public static ChromiumReportFile GetInstance()
        {
            return lazy.Value;
        }
    }
}
