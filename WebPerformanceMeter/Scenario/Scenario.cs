using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebPerformanceMeter.Logger;
using WebPerformanceMeter.PerformancePlans;

namespace WebPerformanceMeter.Scenario
{
    public class Scenario
    {
        protected readonly List<PerformancePlan> PerformancePlans;

        protected readonly Watcher Watcher;

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
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            var watcherTask = Watcher.Processing(token);

            foreach (var performancePlan in PerformancePlans)
            {
                await performancePlan.StartAsync();
            }

            source.Cancel();
            await watcherTask;
        }
    }
}
