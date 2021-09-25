﻿namespace WebPerformanceMeter.Logger.BrowserLog
{
    using System;

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
