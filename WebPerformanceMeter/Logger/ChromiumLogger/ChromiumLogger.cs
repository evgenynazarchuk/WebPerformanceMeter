using System;
using WebPerformanceMeter.DataReader.CsvReader;

namespace WebPerformanceMeter.Logger
{
    public class ChromiumLogger : FileLogger
    {
        public ChromiumLogger() { }

        public override object? GetObjectFromRawCsvLogMessage(string logMessage, Type logMessageType)
        {
            var logMessageObject = CsvConverter.GetObjectFromCsvColumns(logMessage.Split('\t'), logMessageType);
            return logMessageObject;
        }
    }
}
