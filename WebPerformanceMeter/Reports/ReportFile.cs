using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using WebPerformanceMeter.DataReader.CsvReader;
using WebPerformanceMeter.Interfaces;

namespace WebPerformanceMeter.Reports
{
    public abstract class ReportFile : Report
    {
        public ReportFile(string projectName, string testRunId)
            : base(projectName, testRunId)
        {
            this.writers = new();

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

        protected readonly ConcurrentDictionary<string, StreamWriter> writers;

        protected override Task ProcessAsync()
        {
            var task = Task.Run(async () =>
            {
                while (true)
                {
                    if (this.LogQueue.IsEmpty && this.token.IsCancellationRequested)
                    {
                        await this.FinishAsync();

                        break;
                    }

                    if (this.LogQueue.TryDequeue(out (string logName, string logMessage, Type logMessageType) log))
                    {
                        if (!this.writers.TryGetValue(log.logName, out StreamWriter? logWriter))
                        {
                            if (logWriter is null)
                            {
                                logWriter = new StreamWriter(log.logName, false, Encoding.UTF8, 65535);
                            }

                            this.writers.TryAdd(log.logName, logWriter);
                        }
                        else
                        {
                            var jsonLogMessage = this.FromCsvLineToJsonString(log.logMessage, log.logMessageType);

                            //
                            //Console.WriteLine(jsonLogMessage);

                            logWriter.WriteLine(jsonLogMessage);
                        }
                    }
                }
            });

            return task;
        }

        private async Task FinishAsync()
        {
            foreach ((var logName, var fileWriter) in this.writers)
            {
                fileWriter.Flush();
                fileWriter.Close();

                await this.PostProcessingAsync(logName);
            }
        }
    }
}
