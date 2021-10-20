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
            var htmlGenerate = new HttpHtmlReportGenerator("HttpClientToolLog.json", "HttpClientToolReport.html");
            htmlGenerate.GenerateReport();
        }
    }
}
