using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPerformanceMeter.Logger.GrpcLogger
{
    public class GrpcBaseLogMessage
    {
        public string? UserName { get; set; }

        public string? Method { get; set; }

        public string? Label { get; set; }

        public long StartTime { get; set; }

        public long EndTime { get; set; }
    }
}
