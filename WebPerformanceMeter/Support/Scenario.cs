﻿using System;
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
        private readonly List<KeyValuePair<PerformancePlanLaunchType, PerformancePlan[]>> Acts;

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
            AddPlans(PerformancePlanLaunchType.Parallel, performancePlan);
            return this;
        }

        public Scenario AddSequentialPlans(params PerformancePlan[] performancePlan)
        {
            AddPlans(PerformancePlanLaunchType.Sequential, performancePlan);
            return this;
        }

        private Scenario AddPlans(PerformancePlanLaunchType launchType, params PerformancePlan[] performancePlan)
        {
            this.Acts.Add(new(launchType, performancePlan));
            return this;
        }

        public async Task RunAsync()
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;
            var watcherWaiter = Watcher.Processing(token);

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

                if (launchType == PerformancePlanLaunchType.Sequential)
                {
                    foreach (var plan in plans)
                    {
                        await plan.StartAsync();
                    }
                }
                else if (launchType == PerformancePlanLaunchType.Parallel)
                {
                    var plansWaiter = new List<Task>();

                    foreach (var plan in plans)
                    {
                        plansWaiter.Add(Task.Run(async () =>
                        {
                            await plan.StartAsync();
                        }));
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
