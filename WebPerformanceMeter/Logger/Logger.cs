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
    public abstract class Logger
    {
        public Logger()
        {
            this.LogQueue = new();
            this.tokenSource = new();
            this.token = this.tokenSource.Token;
            this._lock = new();
            this._processStart = false;
        }

        public Task StartProcessingAsync()
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

        public void StopProcessing()
        {
            this.tokenSource.Cancel();
        }

        public abstract Task PostProcessingAsync(string logName);

        protected abstract Task ProcessAsync();

        protected readonly CancellationToken token;

        protected readonly CancellationTokenSource tokenSource;

        protected readonly ConcurrentQueue<(string logName, string logMessage, Type logType)> LogQueue;

        private readonly object _lock;

        private bool _processStart;
    }
}
