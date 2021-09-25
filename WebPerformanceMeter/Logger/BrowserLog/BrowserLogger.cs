namespace WebPerformanceMeter.Logger.BrowserLog
{
    using System;
    using WebPerformanceMeter.DataReader.CsvReader;

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
