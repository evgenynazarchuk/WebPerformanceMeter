using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebPerformanceMeter.Logger;
using WebPerformanceMeter.PerformancePlans;
using System.Diagnostics;

namespace WebPerformanceMeter.Support
{
    public class Scenario
    {
        protected readonly List<PerformancePlan> PerformancePlans;

        protected readonly Watcher Watcher;

        public static readonly Stopwatch WatchTime = new();

        public Scenario()
        {
            PerformancePlans = new();
            Watcher = Watcher.Instance.Value;
            Watcher.AddReport(new ConsoleReport());
            Watcher.AddReport(new FileReport());
            Watcher.AddReport(new GrpcReport());
        }

        public Scenario AddPerformancePlan(PerformancePlan performancePlan)
        {
            PerformancePlans.Add(performancePlan);

            return this;
        }

        public async Task RunAsync()
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;
            var watcherTask = Watcher.Processing(token);

            WatchTime.Reset();
            WatchTime.Start();

            foreach (var performancePlan in PerformancePlans)
            {
                await performancePlan.StartAsync();
            }

            // 
            tokenSource.Cancel();
            await watcherTask;

            WatchTime.Stop();
        }
    }
}
