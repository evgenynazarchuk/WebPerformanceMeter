namespace WebPerformanceMeter.Logger.HttpClientLog
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Threading;
    using System.IO;
    using System.Text.Json;

    public class HttpClientLogger : ILogger
    {
        public readonly ConcurrentQueue<string> LogQueue;

        public readonly StreamWriter FileWriter;

        public readonly CancellationToken Token;

        public readonly CancellationTokenSource TokenSource;

        public readonly string RawHttpClientLog;

        private readonly JsonSerializerOptions JsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public HttpClientLogger(string reportFileName)
        {
            this.RawHttpClientLog = reportFileName;
            this.TokenSource = new();
            this.Token = this.TokenSource.Token;

            this.LogQueue = new();
            this.FileWriter = new StreamWriter(reportFileName, false, Encoding.UTF8, 65535);

            ////long reportNumber = DateTime.UtcNow.Ticks;
            ////string targetFolder = $"Logs//{reportNumber}";
            ////
            ////if (!Directory.Exists("Logs"))
            ////{
            ////    Directory.CreateDirectory("Logs");
            ////}
            ////
            ////if (!Directory.Exists(targetFolder))
            ////{
            ////    Directory.CreateDirectory(targetFolder);
            ////}
        }

        public virtual void Write(string message)
        {
            this.LogQueue.Enqueue(message);
        }

        ////public virtual void Processing()
        ////{
        ////    while (true)
        ////    {
        ////        if (this.Token.IsCancellationRequested && this.LogQueue.IsEmpty)
        ////        {
        ////            this.Finish();
        ////            break;
        ////        }
        ////
        ////        this.LogQueue.TryDequeue(out string? message);
        ////
        ////        if (message is not null)
        ////        {
        ////            this.FileWriter.WriteLine(message);
        ////        }
        ////    }
        ////}

        public virtual async Task StartProcessingAsync()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    if (this.Token.IsCancellationRequested && this.LogQueue.IsEmpty)
                    {
                        this.Finish();
                        break;
                    }

                    this.LogQueue.TryDequeue(out string? message);

                    if (message is not null)
                    {
                        var jsonObject = this.GetHttpClientLogMessage(message);
                        var jsonMessage = JsonSerializer.Serialize(jsonObject, this.JsonSerializerOptions);
                        this.FileWriter.WriteLine(jsonMessage);
                    }
                }
            });
        }

        public virtual void StopProcessing()
        {
            this.TokenSource.Cancel();
        }

        public virtual void Finish()
        {
            this.FileWriter.Flush();
            this.FileWriter.Close();
            this.GenerateHtmlReport();
        }

        private HttpClientLogMessage GetHttpClientLogMessage(string message)
        {
            var splittedMessage = message.Split(',');

            HttpClientLogMessage log = new(splittedMessage[0],
                splittedMessage[1],
                splittedMessage[2],
                splittedMessage[3],
                Int32.Parse(splittedMessage[4]),
                Int64.Parse(splittedMessage[5]),
                Int64.Parse(splittedMessage[6]),
                Int64.Parse(splittedMessage[7]),
                Int64.Parse(splittedMessage[8]),
                Int32.Parse(splittedMessage[9]),
                Int32.Parse(splittedMessage[10]));
        
            return log;
        }

        public virtual void GenerateHtmlReport()
        {
            var htmlGenerate = new HttpClientHtmlReportGenerator(this.RawHttpClientLog, "httpclient_report.html");
            ////htmlGenerate.ReadRawLogMessages();
            htmlGenerate.GenerateReport();
        }
    }
}
