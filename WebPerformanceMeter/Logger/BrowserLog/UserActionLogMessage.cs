﻿namespace WebPerformanceMeter.Logger.BrowserLog
{
    public class UserActionLogMessage
    {
        public string? UserName { get; set; }

        public string? Url { get; set; }

        public string? Label { get; set; }

        public long StartTime { get; set; }

        public long EndTime { get; set; }
    }
}
