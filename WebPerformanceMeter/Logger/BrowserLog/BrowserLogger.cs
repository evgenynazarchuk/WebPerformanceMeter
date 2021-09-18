namespace WebPerformanceMeter.Logger.BrowserLog
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Collections.Concurrent;
    using System.IO;
    using System.Text.Json;
    using System.Threading;

    public class BrowserLogger : ILogger
    {
        public readonly ConcurrentQueue<string> LogQueue;

        public readonly StreamWriter FileWriter;

        public readonly CancellationToken Token;

        public readonly CancellationTokenSource TokenSource;

        public readonly string HttpClientLogFileName;

        private readonly JsonSerializerOptions JsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public BrowserLogger(string httpClientLogFileName)
        {
            this.HttpClientLogFileName = httpClientLogFileName;
            this.TokenSource = new();
            this.Token = this.TokenSource.Token;

            this.LogQueue = new();
            this.FileWriter = new StreamWriter(httpClientLogFileName, false, Encoding.UTF8, 65535);

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

        public void Finish()
        {
            this.FileWriter.Flush();
            this.FileWriter.Close();
            //this.GenerateHtmlReport();
        }

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
                        var logMesageEntity = this.GetBrowserLogMessage(message);
                        var logMessageJsonString = JsonSerializer.Serialize(logMesageEntity, this.JsonSerializerOptions);
                        this.FileWriter.WriteLine(logMessageJsonString);
                    }
                }
            });
        }

        private BrowserLogMessage GetBrowserLogMessage(string message)
        {
            var splittedMessage = message.Split(',');

            BrowserLogMessage log = new(splittedMessage[0],
                splittedMessage[1],
                splittedMessage[2],
                Int64.Parse(splittedMessage[3]),
                Int64.Parse(splittedMessage[4]));
            
            return log;
        }

        public void StopProcessing()
        {
            this.TokenSource.Cancel();
        }

        public void AddMessageLog(string message)
        {
            this.LogQueue.Enqueue(message);
        }

        //public virtual void GenerateHtmlReport()
        //{
        //    var htmlGenerate = new HttpClientHtmlReportGenerator(this.HttpClientLogFileName, "httpclient_report.html");
        //    htmlGenerate.GenerateReport();
        //}
    }
}
