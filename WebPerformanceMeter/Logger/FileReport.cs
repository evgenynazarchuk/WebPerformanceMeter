namespace WebPerformanceMeter.Logger
{
    using System;
    using System.Collections.Concurrent;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    public sealed class FileReport : IAsyncReport
    {
        ////private readonly StreamWriter FileStream;

        private readonly string logDictionary;

        ////private readonly string _rawLogFileName;
        ////
        ////private readonly string _htmlReportFileName;

        private readonly ConcurrentDictionary<string, StreamWriter> Writer;

        ////private static readonly JsonSerializerOptions JsonSerializerOptions = new()
        ////{
        ////    PropertyNameCaseInsensitive = true
        ////};

        public FileReport()
        {
            this.Writer = new();
            long reportNumber = DateTime.UtcNow.Ticks;

            if (!Directory.Exists("Logs"))
            {
                Directory.CreateDirectory("Logs");
            }

            if (!Directory.Exists($"Logs//{reportNumber}"))
            {
                Directory.CreateDirectory($"Logs//{reportNumber}");
            }

            this.logDictionary = $"Logs/{reportNumber}/";
            ////_rawLogFileName = $"Logs/{reportNumber}/RawLogMessage.log";
            ////_htmlReportFileName = $"Logs/{reportNumber}/Report.html";

            ////FileStream = new StreamWriter(_rawLogFileName, false, Encoding.UTF8, 65535);
        }

        public async Task WriteAsync(string fileName, string message)
        {
            if (!this.Writer.TryGetValue(this.logDictionary + fileName, out StreamWriter? writer))
            {
                writer?.WriteLine(message);
            }
            else
            {
                var newWriter = new StreamWriter(fileName, false, Encoding.UTF8, 65535);
                this.Writer.TryAdd(this.logDictionary + fileName, newWriter);
                newWriter.WriteLine(message);
            }

            //var log = GetHttpClientLogEntity(message);
            //var serializedLog = JsonSerializer.Serialize(log, JsonSerializerOptions);
            //
            //FileStream.WriteLine(serializedLog);
            await Task.CompletedTask;
        }

        ////private HttpClientLogMessage GetHttpClientLogEntity(string message)
        ////{
        ////    var splittedMessage = message.Split(',');
        ////    HttpClientLogMessage log = new(splittedMessage[0],
        ////        splittedMessage[1],
        ////        splittedMessage[2],
        ////        splittedMessage[3],
        ////        Int32.Parse(splittedMessage[4]),
        ////        Int64.Parse(splittedMessage[5]),
        ////        Int64.Parse(splittedMessage[6]),
        ////        Int64.Parse(splittedMessage[7]),
        ////        Int64.Parse(splittedMessage[8]),
        ////        Int32.Parse(splittedMessage[9]),
        ////        Int32.Parse(splittedMessage[10]));
        ////
        ////    return log;
        ////}

        public void Finish()
        {
            foreach (var (_, v) in this.Writer)
            {
                v.Flush();
                v.Close();
            }

            ////FileStream.Flush();
            ////FileStream.Close();

            ////var htmlGenerate = new HttpClientHtmlReportGenerator(_rawLogFileName, _htmlReportFileName);
            ////htmlGenerate.ReadRawLogMessages();
            ////htmlGenerate.GenerateReport();
        }
    }
}
