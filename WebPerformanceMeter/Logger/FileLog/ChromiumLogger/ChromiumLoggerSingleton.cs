using System;

namespace WebPerformanceMeter.Report
{
    public static class ChromiumLoggerSingleton
    {
        private static readonly Lazy<ChromiumLogger> lazy = new(() => new());

        public static ChromiumLogger GetInstance()
        {
            return lazy.Value;
        }
    }
}
