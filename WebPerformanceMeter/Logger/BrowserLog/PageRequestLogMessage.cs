namespace WebPerformanceMeter.Logger.BrowserLog
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class PageRequestLogMessage
    {
        public string? UserName { get; set; }

        public string? HttpMethod { get; set; }

        public string? Request { get; set; }

        public float? RequestStart { get; set; }

        public float? ResponseStart {  get; set; }

        public float? ResponseEnd {  get; set; }
    }
}
