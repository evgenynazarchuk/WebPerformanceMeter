using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using WebPerformanceMeter.DataReader.CsvReader;
using WebPerformanceMeter.Interfaces;

namespace WebPerformanceMeter.Logger
{
    public abstract class FileLogger : ILogger
    {
        public FileLogger()
        {
            this.TokenSource = new();
            this.Token = this.TokenSource.Token;

            this.logQueue = new();
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

        public readonly CancellationToken Token;

        public readonly CancellationTokenSource TokenSource;

        protected readonly ConcurrentQueue<(string logName, string logMessage, Type logType)> logQueue;

        protected readonly ConcurrentDictionary<string, StreamWriter> writers;

        public ConcurrentQueue<(string logName, string logMessage, Type logType)> LogQueue { get => this.logQueue; }

        public ConcurrentDictionary<string, StreamWriter> Writers { get => this.writers; }

        private readonly object _lock = new object();

        private bool _processStart = false;

        public virtual Task StartAsync()
        {
            lock (this._lock)
            {
                if (this._processStart)
                {
                    return Task.CompletedTask;
                }
                else
                {
                    _processStart = true;
                }
            }

            return Task.Run(() =>
            {
                while (true)
                {
                    if (this.logQueue.IsEmpty && this.Token.IsCancellationRequested)
                    {
                        break;
                    }

                    if (this.logQueue.TryDequeue(out (string logName, string logMessage, Type logMessageType) log))
                    {
                        if (!this.writers.TryGetValue(log.logName, out StreamWriter? logWriter))
                        {
                            if (logWriter is null)
                            {
                                logWriter = new StreamWriter(log.logName, false, Encoding.UTF8, 65535);
                            }

                            this.writers.TryAdd(log.logName, logWriter);
                        }

                        if (logWriter is not null)
                        {
                            var jsonLogMessage = this.Convert(log.logMessage, log.logMessageType);

                            //
                            //Console.WriteLine(jsonLogMessage);

                            logWriter.WriteLine(jsonLogMessage);
                        }
                    }
                }

                this.Finish();
            });
        }

        public virtual void ProcessStop()
        {
            this.TokenSource.Cancel();
        }

        public virtual void Finish()
        {
            foreach ((var logName, var fileWriter) in this.writers)
            {
                fileWriter.Flush();
                fileWriter.Close();

                this.PostProcessing(logName);
            }

            this.PostProcessing();
        }

        // after finish
        public virtual void PostProcessing(string logName)
        {
        }

        public virtual void PostProcessing()
        {
        }

        public virtual void AddLogMessage(string logName, string logMessage, Type logMessageType)
        {
            //
            //Console.WriteLine($"log message: {logName} {logMessage}");

            this.logQueue.Enqueue((logName, logMessage, logMessageType));
        }

        // from string to string
        public virtual string Convert(string logMessage, Type logMessageType)
        {
            var obj = this.GetObjectFromRawCsvLogMessage(logMessage, logMessageType);
            var jsonString = this.GetJsonStringFromObject(obj, logMessageType);

            return jsonString;
        }

        // from csv text to object
        public virtual object? GetObjectFromRawCsvLogMessage(string logMessage, Type logMessageType)
        {
            var logMessageObject = CsvConverter.GetObjectFromCsvLine(logMessage, logMessageType);

            return logMessageObject;
        }

        // from object to json string
        public virtual string GetJsonStringFromObject(object? logMessageObject, Type logMessageType)
        {
            var jsonString = JsonSerializer.Serialize(logMessageObject, logMessageType);
            return jsonString;
        }
    }
}
