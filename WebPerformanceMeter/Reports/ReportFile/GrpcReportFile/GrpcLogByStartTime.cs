using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPerformanceMeter.Reports
{
    public class GrpcLogByStartTime : GrpcLogByTime
    {
        public GrpcLogByStartTime(
            string? userName,
            string? method,
            string? label,
            long time,
            long count)
            : base(userName, method, label, time, count) { }
    }
}
