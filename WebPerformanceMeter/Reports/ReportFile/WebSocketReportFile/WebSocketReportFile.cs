using System.Threading.Tasks;

namespace WebPerformanceMeter.Reports
{
    public class WebSocketReportFile : ReportFile
    {
        public WebSocketReportFile(string projectName, string testRunId)
            : base(projectName, testRunId) { }

        protected override Task PostProcessingAsync(string logName)
        {
            if (logName == "WebSocketLogMessage.json")
            {
                var htmlGenerate = new WebSocketReportHtmlBuilder(logName, "WebSocketLogMessage.html");
                htmlGenerate.Build();
            }

            return Task.CompletedTask;
        }
    }
}
