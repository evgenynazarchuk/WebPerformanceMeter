namespace WebPerformanceMeter.Logger
{
    public class HttpLogger : FileLogger
    {
        public HttpLogger() { }

        public override void PostProcessing()
        {
            GenerateHtmlReport();
        }

        public void GenerateHtmlReport()
        {
            var htmlGenerate = new HttpLogMessageHtmlBuilder("HttpClientToolLog.json", "HttpClientToolReport.html");
            htmlGenerate.BuildHtml();
        }
    }
}
