using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPerformanceMeter.Logger.BrowserLog
{
    public class BrowserLogMessage
    {
        public BrowserLogMessage(
            string url,
            string userName,
            string label,
            long startTime,
            long endTime
            )
        {
            this.Url = url;
            this.UserName = userName;
            this.Label = label;
            this.StartTime = startTime;
            this.EndTime = endTime;
        }

        public string UserName { get; set; }

        public string Url { get; set; }

        public string Label { get; set; }

        public long StartTime {  get; set; }

        public long EndTime {  get; set; }
    }
}
