using System.Threading.Tasks;

namespace WebPerformanceMeter.Report
{
    public class HttpLogger : FileLogger
    {
        public HttpLogger() { }

        protected override Task PostProcessingAsync(string logName)
        {
            if (logName == "HttpClientToolLog.json")
            {
                var htmlGenerate = new HttpLogMessageHtmlBuilder("HttpClientToolLog.json", "HttpClientToolReport.html");
                htmlGenerate.BuildHtml();
            }

            return Task.CompletedTask;
        }
    }
}
