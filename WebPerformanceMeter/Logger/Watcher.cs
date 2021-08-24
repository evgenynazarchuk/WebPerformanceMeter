using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WebPerformanceMeter.Logger
{
    public class Watcher
    {
        public readonly ConcurrentQueue<string> Queue;

        public readonly List<AsyncReport> Reports;

        public static Lazy<Watcher> Instance = new (() => new ());

        public Watcher()
        {
            Queue = new();
            Reports = new();
        }

        public void AddReport(AsyncReport report)
        {
            Reports.Add(report);
        }

        public void Send(string message)
        {
            Queue.Enqueue(message);
        }

        public Task Processing(CancellationToken token)
        {
            return Task.Run(async () =>
            {
                while (true)
                {
                    if (token.IsCancellationRequested && Queue.IsEmpty)
                    {
                        foreach (var report in Reports)
                        {
                            report.Finish();
                        }

                        return;
                    }

                    Queue.TryDequeue(out string? message);

                    if (message is not null)
                    {
                        foreach (var report in Reports)
                        {
                            await report.WriteAsync(message);
                        }
                    }
                }
            });
        }
    }
}
