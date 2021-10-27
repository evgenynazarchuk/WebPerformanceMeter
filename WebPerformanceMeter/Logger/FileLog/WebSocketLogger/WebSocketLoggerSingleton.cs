using System;

namespace WebPerformanceMeter.Report
{
    public class WebSocketLoggerSingleton
    {
        private static readonly Lazy<WebSocketLogger> lazy = new(() => new());

        public static WebSocketLogger GetInstance()
        {
            return lazy.Value;
        }
    }
}
