using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using WebPerformanceMeter.Logger;
using WebPerformanceMeter.PerformancePlans;

namespace WebPerformanceMeter.Support
{
    public sealed class Scenario
    {
        private readonly List<KeyValuePair<ActType, PerformancePlan[]>> Acts;

        private readonly Watcher Watcher;

        public static readonly Stopwatch ScenarioWatchTime = new();

        public Scenario()
        {
            this.Acts = new();
            Watcher = Watcher.Instance.Value;
            Watcher.AddReport(new ConsoleReport());
            Watcher.AddReport(new FileReport());
            Watcher.AddReport(new GrpcReport());
        }

        public Scenario AddParallelPlans(params PerformancePlan[] performancePlan)
        {
            AddActs(ActType.Parallel, performancePlan);
            return this;
        }

        public Scenario AddSequentialPlans(params PerformancePlan[] performancePlan)
        {
            AddActs(ActType.Sequential, performancePlan);
            return this;
        }

        private Scenario AddActs(ActType launchType, params PerformancePlan[] performancePlan)
        {
            this.Acts.Add(new(launchType, performancePlan));
            return this;
        }

        public async Task StartAsync()
        {
            CancellationTokenSource tokenSource = new();
            CancellationToken token = tokenSource.Token;
            Task watcherWaiter = Watcher.Processing(token);

            if (this.Acts.Count == 0)
            {
                throw new ApplicationException("UserPerformancePlan not added");
            }

            ScenarioWatchTime.Reset();
            ScenarioWatchTime.Start();

            foreach (var act in this.Acts)
            {
                var launchType = act.Key;
                var plans = act.Value;

                if (launchType == ActType.Sequential)
                {
                    foreach (var plan in plans)
                    {
                        await plan.StartAsync();
                    }
                }
                else if (launchType == ActType.Parallel)
                {
                    var plansWaiter = new List<Task>();

                    foreach (var plan in plans)
                    {
                        plansWaiter.Add(Task.Run(async () =>
                        {
                            await plan.StartAsync();
                        }));

                        //plansWaiter.Add(plan.StartAsync()); // do not work!?
                    }

                    Task.WaitAll(plansWaiter.ToArray());
                }
            }

            // 
            tokenSource.Cancel();
            await watcherWaiter;

            ScenarioWatchTime.Stop();
        }
    }
}
