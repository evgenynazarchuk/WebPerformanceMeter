using System;

namespace WebPerformanceMeter.Logger
{
    public class GrpcLoggerSingleton
    {
        private static readonly Lazy<GrpcLogger> lazy = new(() => new());

        public static GrpcLogger GetInstance()
        {
            return lazy.Value;
        }
    }
}
