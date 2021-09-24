namespace WebPerformanceMeter.Logger.HttpClientLog
{
    using System;
    using System.Collections.Concurrent;
    using System.IO;
    using System.Text;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;

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
