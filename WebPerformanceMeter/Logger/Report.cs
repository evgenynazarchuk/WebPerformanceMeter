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
    public abstract class Report : ILogger
    {
        public Report()
        {
            this.LogQueue = new();
            this.tokenSource = new();
            this.token = this.tokenSource.Token;
            this._lock = new();
            this._processStart = false;
        }

        public Task StartAsync()
        {
            lock (this._lock)
            {
                if (this._processStart is true)
                {
                    return Task.CompletedTask;
                }
                else
                {
                    _processStart = true;
                }
            }

            return this.ProcessAsync();
        }

        public void Stop()
        {
            this.tokenSource.Cancel();
        }

        public virtual void SendLogMessage(string logName, string logMessage, Type logMessageType)
        {
            //
            //Console.WriteLine($"log message: {logName} {logMessage}");

            this.LogQueue.Enqueue((logName, logMessage, logMessageType));
        }

        protected virtual Task PostProcessingAsync(string logName)
        {
            return Task.CompletedTask;
        }

        // from csv line to object
        protected virtual object? FromCsvLineToObject(string logMessageCsvLine, Type logMessageType)
        {
            var logMessageObject = CsvConverter.GetObjectFromCsvLine(logMessageCsvLine, logMessageType);

            return logMessageObject;
        }

        // from csv line to json string
        protected virtual string FromCsvLineToJsonString(string logMessageCsvLine, Type logMessageType)
        {
            var obj = this.FromCsvLineToObject(logMessageCsvLine, logMessageType);
            var jsonString = this.FromObjectToJsonString(obj, logMessageType);

            return jsonString;
        }

        // from object to json string
        protected virtual string FromObjectToJsonString(object? logMessageObject, Type logMessageType)
        {
            var jsonString = JsonSerializer.Serialize(logMessageObject, logMessageType);
            return jsonString;
        }

        protected abstract Task ProcessAsync();

        protected readonly CancellationToken token;

        protected readonly CancellationTokenSource tokenSource;

        protected readonly ConcurrentQueue<(string logName, string logMessage, Type logType)> LogQueue;

        private readonly object _lock;

        private bool _processStart;
    }
}
