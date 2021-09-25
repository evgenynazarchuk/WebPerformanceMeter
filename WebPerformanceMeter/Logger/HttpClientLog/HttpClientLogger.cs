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
            var htmlGenerate = new HttpClientToolLogHtmlReportGenerator("HttpClientLog.json", "HttpClientReport.html");
            htmlGenerate.GenerateReport();
        }
    }
}
