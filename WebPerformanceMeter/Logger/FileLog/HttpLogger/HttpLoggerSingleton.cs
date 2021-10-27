using System;

namespace WebPerformanceMeter.Report
{
    public static class HttpLoggerSingleton
    {
        private static readonly Lazy<HttpLogger> lazy = new(() => new());

        public static HttpLogger GetInstance()
        {
            return lazy.Value;
        }
    }
}
