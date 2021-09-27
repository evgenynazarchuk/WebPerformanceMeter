using System;
using WebPerformanceMeter.DataReader.CsvReader;

namespace WebPerformanceMeter.Logger.BrowserLog
{
    public class BrowserLogger : FileLogger
    {
        public BrowserLogger() { }

        public override object? GetObjectFromRawCsvLogMessage(string logMessage, Type logMessageType)
        {
            var logMessageObject = CsvConverter.GetObjectFromCsvColumns(logMessage.Split('\t'), logMessageType);
            return logMessageObject;
        }
    }
}
