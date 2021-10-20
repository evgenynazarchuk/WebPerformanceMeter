using System;

namespace WebPerformanceMeter.Logger.HttpClientLog
{
    public static class HttpClientLoggerSingleton
    {
        private static readonly Lazy<HttpClientLogger> lazy = new(() => new());

        public static HttpClientLogger GetInstance()
        {
            return lazy.Value;
        }
    }
}
