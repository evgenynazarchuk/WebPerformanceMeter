using System;

namespace WebPerformanceMeter.Logger
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
