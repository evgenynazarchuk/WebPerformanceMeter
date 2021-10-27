using System.Threading.Tasks;

namespace WebPerformanceMeter.Reports
{
    public class HttpReportFile : ReportFile
    {
        public HttpReportFile(string projectName, string testRunId)
            : base(projectName, testRunId) { }

        protected override Task PostProcessingAsync(string logName)
        {
            if (logName == "HttpClientToolLog.json")
            {
                var htmlGenerate = new HttpRequestHtmlBuilder(logName, "HttpClientToolReport.html");
                htmlGenerate.Build();
            }

            return Task.CompletedTask;
        }
    }
}
