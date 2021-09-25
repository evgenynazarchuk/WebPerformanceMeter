namespace WebPerformanceMeter.Logger.BrowserLog
{
    public class PageRequestLogMessage
    {
        public string? UserName { get; set; }

        public string? HttpMethod { get; set; }

        public string? Request { get; set; }

        public float? RequestStart { get; set; }

        public float? ResponseStart { get; set; }

        public float? ResponseEnd { get; set; }
    }
}
