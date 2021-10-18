using System;

namespace WebPerformanceMeter.Logger.BrowserLog
{
    public class PageRequestLogMessage
    {
        public string? UserName { get; set; }

        public string? HttpMethod { get; set; }

        public string? Request { get; set; }

        public TimeSpan? RequestStart { get; set; }

        public TimeSpan? ResponseStart { get; set; }

        public TimeSpan? ResponseEnd { get; set; }
    }
}
