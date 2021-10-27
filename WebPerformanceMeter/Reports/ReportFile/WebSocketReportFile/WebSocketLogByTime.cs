using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPerformanceMeter.Reports
{
    public class WebSocketLogByTime
    {
        public WebSocketLogByTime(
            string? userName,
            string? actionType,
            string? label,
            long time,
            long count)
        {
            this.UserName = userName;
            this.ActionType = actionType;
            this.Label = label;
            this.Time = time;
            this.Count = count;
        }

        public string? UserName { get; set; }

        public string? ActionType { get; set; }

        public string? Label { get; set; }

        public long Time { get; set; }

        public long Count { get; set; }
    }
}
