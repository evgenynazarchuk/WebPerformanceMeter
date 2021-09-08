namespace WebPerformanceMeter.Logger
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class Watcher
    {
        public static Lazy<Watcher> Instance = new(() => new());

        public readonly ConcurrentDictionary<string, ConcurrentQueue<string>> LogQueue;

        public readonly List<IAsyncReport> Reports;

        public Watcher()
        {
            this.LogQueue = new();
            this.Reports = new();
        }

        public void AddReport(IAsyncReport report)
        {
            Reports.Add(report);
        }

        public void Send(string fileName, string message)
        {
            ////Console.WriteLine($"Send {fileName} {message}"); // OK
            if (this.LogQueue.TryGetValue(fileName, out ConcurrentQueue<string>? logQueue))
            {
                ////Console.WriteLine($"if TryGetValue {fileName} {message}"); // OK
                logQueue.Enqueue(message);
            }
            else
            {
                ////Console.WriteLine($"else TryGetValue {fileName} {message}"); // Bad
                var newLogQueue = new ConcurrentQueue<string>();

                if (this.LogQueue.TryAdd(fileName, newLogQueue))
                {
                    ////Console.WriteLine($"TryAdd {fileName} {message}"); //
                    newLogQueue.Enqueue(message);
                }
                else
                {
                    throw new ApplicationException("Error add new Log Queue");
                }
            }
        }

        public async Task ProcessingAsync(CancellationToken token)
        {
            List<Task> writerTasks = new();

            foreach (var (fileName, queue) in this.LogQueue)
            {
                writerTasks.Add(Task.Run(async () =>
                {
                    while (true)
                    {
                        if (token.IsCancellationRequested && queue.IsEmpty)
                        {
                            foreach (var report in this.Reports)
                            {
                                report.Finish();
                            }

                            break;
                        }

                        queue.TryDequeue(out string? message);

                        if (message is not null)
                        {
                            foreach (var report in this.Reports)
                            {
                                await report.WriteAsync(fileName, message);
                            }
                        }
                    }
                }));

                Task.WaitAll(writerTasks.ToArray());
                await Task.CompletedTask;
            }
        }
    }
}
