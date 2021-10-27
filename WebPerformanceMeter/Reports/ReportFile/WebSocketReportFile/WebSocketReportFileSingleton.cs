using System;

namespace WebPerformanceMeter.Reports
{
    public class WebSocketReportFileSingleton
    {
        private static readonly Lazy<WebSocketReportFile> lazy = new(() => new());

        public static WebSocketReportFile GetInstance()
        {
            return lazy.Value;
        }
    }
}
