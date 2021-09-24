using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.IO;
using System.Text.Json;
using System.Threading;

namespace WebPerformanceMeter.Logger
{
    public abstract class PerformanceLogger : IPerformanceLogger
    {
        public PerformanceLogger(string fileNamePrefix)
        {
            this.UserLogFileName = fileNamePrefix + "_user.log";
            this.ToolLogFileName = fileNamePrefix + "_tool.log";

            this.UserLogQueue = new();
            this.ToolLogQueue = new();

            this.ToolFileWriter = new StreamWriter(this.ToolLogFileName, false, Encoding.UTF8, 65535);
            this.UserFileWriter = new StreamWriter(this.UserLogFileName, false, Encoding.UTF8, 65535);

            this.TokenSource = new();
            this.Token = this.TokenSource.Token;

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

        protected readonly JsonSerializerOptions JsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public readonly ConcurrentQueue<string> ToolLogQueue;

        public readonly ConcurrentQueue<string> UserLogQueue;

        public readonly StreamWriter ToolFileWriter;

        public readonly StreamWriter UserFileWriter;

        public readonly CancellationToken Token;

        public readonly CancellationTokenSource TokenSource;

        public readonly string ToolLogFileName;

        public readonly string UserLogFileName;

        public virtual void AppendUserLogMessage(string message)
        {
            this.UserLogQueue.Enqueue(message);
        }

        public virtual void AppendToolLogMessage(string message)
        {
            this.ToolLogQueue.Enqueue(message);
        }

        public virtual async Task StartProcessingAsync()
        {
            await Task.Run(() =>
            {
                var userWriter = Task.Run(() =>
                {
                    while (true)
                    {
                        if (this.Token.IsCancellationRequested && this.UserLogQueue.IsEmpty)
                        {
                            break;
                        }
                        if (this.UserLogQueue.TryDequeue(out string? message))
                        {
                            this.UserWriteLogSerialize(message);
                        }
                    }
                });

                var toolWriter = Task.Run(() =>
                {
                    while (true)
                    {
                        if (this.Token.IsCancellationRequested && this.ToolLogQueue.IsEmpty)
                        {
                            break;
                        }
                        if (this.ToolLogQueue.TryDequeue(out string? message))
                        {
                            this.ToolWriteLogSerialize(message);
                        }
                    }
                });

                Task.WaitAll(userWriter, toolWriter);
                this.Finish();
            });
        }

        public virtual void StopProcessing()
        {
            this.TokenSource.Cancel();
        }

        public virtual void UserWriteLogSerialize(string message)
        {
            this.UserFileWriter.WriteLine(message);
        }

        public virtual void ToolWriteLogSerialize(string message)
        {
            this.ToolFileWriter.WriteLine(message);
        }

        public virtual void Finish()
        {
            this.UserFileWriter.Flush();
            this.UserFileWriter.Close();
            this.ToolFileWriter.Flush();
            this.ToolFileWriter.Close();

            this.PostProcessing();
        }

        public virtual void PostProcessing()
        {
        }
    }
}
