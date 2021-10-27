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
                var htmlGenerate = new HttpReportHtmlFile("HttpClientToolLog.json", "HttpClientToolReport.html");
                htmlGenerate.BuildHtml();
            }

            return Task.CompletedTask;
        }
    }
}
