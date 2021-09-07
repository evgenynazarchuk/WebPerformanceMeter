using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WebPerformanceMeter.Logger
{
    public class Watcher
    {
        public readonly ConcurrentQueue<string> HttpClientActionLogQueue;

        public readonly ConcurrentQueue<string> BrowserActionLogQueue;

        public readonly ConcurrentQueue<string> JavascriptActionLogQueue;

        public readonly ConcurrentQueue<string> GrpcActionLogQueue;

        public readonly ConcurrentQueue<string> WebSocketActionLogQueue;

        public readonly List<AsyncReport> Reports;

        public static Lazy<Watcher> Instance = new(() => new());

        public Watcher()
        {
            HttpClientActionLogQueue = new();
            BrowserActionLogQueue = new();
            JavascriptActionLogQueue = new();
            GrpcActionLogQueue = new();
            WebSocketActionLogQueue = new();
            Reports = new();
        }

        public void AddReport(AsyncReport report)
        {
            Reports.Add(report);
        }

        public void SendFromHttpClient(string message)
        {
            HttpClientActionLogQueue.Enqueue(message);
        }

        public void SendFromBrowser(string message)
        {
            this.BrowserActionLogQueue.Enqueue(message);
        }

        public Task HttpClientLogProcessing(CancellationToken token)
        {
            return Task.Run(async () =>
            {
                while (true)
                {
                    if (token.IsCancellationRequested && this.HttpClientActionLogQueue.IsEmpty)
                    {
                        foreach (var report in this.Reports)
                        {
                            report.Finish();
                        }

                        return;
                    }

                    this.HttpClientActionLogQueue.TryDequeue(out string? message);
                    if (message is not null)
                    {
                        foreach (var report in this.Reports)
                        {
                            await report.WriteAsync(message);
                        }
                    }
                }
            });
        }

        public Task BrowserActionLogProcessing(CancellationToken token)
        {
            return Task.Run(async () =>
            {
                while (true)
                {
                    if (token.IsCancellationRequested && this.BrowserActionLogQueue.IsEmpty)
                    {
                        foreach (var report in this.Reports)
                        {
                            report.Finish();
                        }

                        return;
                    }

                    this.BrowserActionLogQueue.TryDequeue(out string? message);
                    if (message is not null)
                    {
                        foreach (var report in this.Reports)
                        {
                            await report.WriteAsync(message);
                        }
                    }
                }
            });
        }
    }
}
