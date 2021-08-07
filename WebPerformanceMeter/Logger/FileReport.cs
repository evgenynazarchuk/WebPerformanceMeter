using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebPerformanceMeter.Logger
{
    public sealed class FileReport : AsyncReport
    {
        private readonly StreamWriter FileStream;

        private const string _rawLogFileName = "RawLogMessage.log";

        private const string _htmlReportFileName = "Report.html";

        private static readonly JsonSerializerOptions JsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public FileReport()
        {
            FileStream = new StreamWriter(_rawLogFileName, false, Encoding.UTF8, 65535);
        }

        public override async Task WriteAsync(string message)
        {
            var log = GetLogEntity(message);
            var serializedLog = JsonSerializer.Serialize(log, JsonSerializerOptions);

            FileStream.WriteLine(serializedLog);
            await Task.CompletedTask;
        }

        private LogMessage GetLogEntity(string message)
        {
            var splittedMessage = message.Split(',');
            LogMessage log = new(
                splittedMessage[0],
                splittedMessage[1],
                splittedMessage[2],
                splittedMessage[3],
                Int32.Parse(splittedMessage[4]),
                Int64.Parse(splittedMessage[5]),
                Int64.Parse(splittedMessage[6]),
                Int64.Parse(splittedMessage[7]),
                Int64.Parse(splittedMessage[8]),
                Int32.Parse(splittedMessage[9]),
                Int32.Parse(splittedMessage[10])
                );

            return log;
        }

        public override void Finish()
        {
            FileStream.Flush();
            FileStream.Close();

            var htmlGenerate = new HtmlGenerator(_rawLogFileName, _htmlReportFileName);
            htmlGenerate.ReadRawLogMessages();
            htmlGenerate.GenerateReport();
        }

        public void GenerateHtmlReport()
        { 
        }
    }
}
