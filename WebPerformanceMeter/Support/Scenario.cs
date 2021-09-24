namespace WebPerformanceMeter.Support
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using WebPerformanceMeter.PerformancePlans;

    public sealed class Scenario
    {
        private readonly List<KeyValuePair<ActType, PerformancePlan[]>> acts;

        private readonly List<Task> loggersProcessing;

        public Scenario()
        {
            this.acts = new();
            this.loggersProcessing = new();
        }

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
            this.StartLoggers();

            ScenarioTimer.Time.Start();

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

            ScenarioTimer.Time.Stop();

            this.WaitLoggers();
        }

        public void StartLoggers()
        {
            foreach (var (_, plans) in this.acts)
            {
                foreach (var plan in plans)
                {
                    loggersProcessing.Add(Task.Run(() => plan.User.Logger.ProcessStart()));
                }
            }
        }

        public void WaitLoggers()
        {
            foreach (var (_, plans) in this.acts)
            {
                foreach (var plan in plans)
                {
                    plan.User.Logger.ProcessStop();
                }
            }

            Task.WaitAll(loggersProcessing.ToArray());
        }
    }
}
