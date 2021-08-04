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

        private readonly StreamWriter ErrorFileStream;

        private static readonly JsonSerializerOptions JsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public FileReport()
        {
            FileStream = new StreamWriter("result.log", false, Encoding.UTF8, 65535);
            ErrorFileStream = new StreamWriter("error.log", false, Encoding.UTF8, 65535);
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
                Int32.Parse(splittedMessage[1]),
                DateTime.Parse(splittedMessage[2]),
                DateTime.Parse(splittedMessage[3]),
                DateTime.Parse(splittedMessage[4]),
                DateTime.Parse(splittedMessage[5]),
                Int32.Parse(splittedMessage[6]),
                Int32.Parse(splittedMessage[7])
                );

            return log;
        }

        public override void Finish()
        {
            FileStream.Flush();
            FileStream.Close();
            ErrorFileStream.Flush();
            ErrorFileStream.Close();
        }

        public override async Task WriteErrorAsync(string message)
        {
            ErrorFileStream.WriteLine(message);
            await Task.CompletedTask;
        }
    }
}
