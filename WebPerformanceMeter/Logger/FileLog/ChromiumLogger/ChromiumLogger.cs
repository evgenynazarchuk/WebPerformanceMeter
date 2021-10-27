using System;
using WebPerformanceMeter.DataReader.CsvReader;

namespace WebPerformanceMeter.Report
{
    public class ChromiumLogger : FileLogger
    {
        public ChromiumLogger() { }

        protected override object? FromCsvLineToObject(string logMessage, Type logMessageType)
        {
            var logMessageObject = CsvConverter.GetObjectFromCsvColumns(logMessage.Split('\t'), logMessageType);
            return logMessageObject;
        }
    }
}
