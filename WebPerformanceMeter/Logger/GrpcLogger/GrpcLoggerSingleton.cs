using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPerformanceMeter.Logger.GrpcLogger
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
