using System;

namespace WebPerformanceMeter.Logger.BrowserLog
{
    public static class BrowserLoggerSingleton
    {
        private static readonly Lazy<BrowserLogger> lazy = new(() => new());

        public static BrowserLogger GetInstance()
        {
            return lazy.Value;
        }
    }
}
