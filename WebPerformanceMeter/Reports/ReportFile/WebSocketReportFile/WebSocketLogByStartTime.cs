using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPerformanceMeter.Reports
{
    public class WebSocketLogByStartTime : WebSocketLogByTime
    {
        public WebSocketLogByStartTime(
            string? userName,
            string? actionType,
            string? label,
            long time,
            long count)
            : base(userName, actionType, label, time, count) { }
    }
}
