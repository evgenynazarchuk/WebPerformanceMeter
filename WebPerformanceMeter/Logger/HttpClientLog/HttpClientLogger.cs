namespace WebPerformanceMeter.Logger.HttpClientLog
{
    public class HttpClientLogger : FileLogger
    {
        public HttpClientLogger() { }

        public override void PostProcessing()
        {
            GenerateHtmlReport();
        }

        public void GenerateHtmlReport()
        {
            var htmlGenerate = new HttpClientToolLogHtmlReportGenerator("HttpClientToolLog.json", "HttpClientToolReport.html");
            htmlGenerate.GenerateReport();
        }
    }
}
