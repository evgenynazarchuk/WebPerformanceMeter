namespace WebPerformanceMeter.Support
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using WebPerformanceMeter.Logger;
    using WebPerformanceMeter.PerformancePlans;

    public sealed class Scenario
    {
        //public static readonly Stopwatch ScenarioWatchTime = new();

        public Scenario()
        {
            this.acts = new();
            this.watcher = Watcher.Instance.Value;
            this.watcher.AddReport(new ConsoleReport());
            this.watcher.AddReport(new FileReport());
        }

        private readonly List<KeyValuePair<ActType, PerformancePlan[]>> acts;

        private readonly Watcher watcher;

        public Scenario AddParallelPlans(params PerformancePlan[] performancePlan)
        {
            this.AddActs(ActType.Parallel, performancePlan);
            return this;
        }

        public Scenario AddSequentialPlans(params PerformancePlan[] performancePlan)
        {
            this.AddActs(ActType.Sequential, performancePlan);
            return this;
        }

        private Scenario AddActs(ActType launchType, params PerformancePlan[] performancePlan)
        {
            this.acts.Add(new(launchType, performancePlan));
            return this;
        }

        public async Task StartAsync()
        {
            //CancellationTokenSource tokenSource = new();
            //CancellationToken token = tokenSource.Token;
            //Task httpClientProcessing = this.watcher.ProcessingAsync(token);

            if (this.acts.Count == 0)
            {
                throw new ApplicationException("UserPerformancePlan not added");
            }

            List<Task> loggersProcessing = new();
            foreach (var (_, plans) in this.acts)
            {
                foreach (var plan in plans)
                {
                    loggersProcessing.Add(Task.Run(() => plan.User.Logger.StartProcessingAsync()));
                }
            }

            ////ScenarioTimer.Time.Reset();
            ScenarioTimer.Time.Start();

            //ScenarioWatchTime.Reset();
            //ScenarioWatchTime.Start();

            foreach (var act in this.acts)
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
                    }

                    Task.WaitAll(plansWaiter.ToArray());
                }
            }

            //tokenSource.Cancel();
            foreach (var (_, plans) in this.acts)
            {
                foreach (var plan in plans)
                {
                    plan.User.Logger.StopProcessing();
                }
            }
            Task.WaitAll(loggersProcessing.ToArray());
            //await httpClientProcessing;

            ////ScenarioWatchTime.Stop();
            ScenarioTimer.Time.Stop();
        }
    }
}
