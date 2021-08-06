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

        private static readonly JsonSerializerOptions JsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public FileReport()
        {
            FileStream = new StreamWriter("result.log", false, Encoding.UTF8, 65535);
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
                Int32.Parse(splittedMessage[2]),
                Int64.Parse(splittedMessage[3]),
                Int64.Parse(splittedMessage[4]),
                Int64.Parse(splittedMessage[5]),
                Int64.Parse(splittedMessage[6]),
                Int32.Parse(splittedMessage[7]),
                Int32.Parse(splittedMessage[8])
                );

            return log;
        }

        public override void Finish()
        {
            FileStream.Flush();
            FileStream.Close();
        }
    }
}
