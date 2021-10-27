using System.Threading.Tasks;

namespace WebPerformanceMeter.Reports
{
    public class GrpcReportFile : ReportFile
    {
        public GrpcReportFile(string projectName, string testRunId)
            : base(projectName, testRunId) { }

        protected override Task PostProcessingAsync(string logName)
        {
            if (logName == "GrpcLogMessage.json")
            {
                var htmlGenerate = new GrpcReportHtmlBuilder(logName, "GrpcLogMessage.Html");
                htmlGenerate.Build();
            }

            return Task.CompletedTask;
        }
    }
}
