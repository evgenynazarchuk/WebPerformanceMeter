using System;
using WebPerformanceMeter.DataReader.CsvReader;

namespace WebPerformanceMeter.Reports
{
    public class ChromiumReportFile : ReportFile
    {
        public ChromiumReportFile(string projectName, string testRunId)
        : base(projectName, testRunId) { }

        protected override object? FromCsvLineToObject(string logMessage, Type logMessageType)
        {
            var logMessageObject = CsvConverter.GetObjectFromCsvColumns(logMessage.Split('\t'), logMessageType);
            return logMessageObject;
        }
    }
}
