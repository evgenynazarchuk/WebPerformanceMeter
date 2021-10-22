using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebPerformanceMeter.PerformancePlans;

namespace WebPerformanceMeter.Support
{
    public sealed class Scenario
    {
        private readonly List<KeyValuePair<ActType, IUsersPerformancePlan[]>> _acts;

        private readonly List<Task> _taskLoggers;

        public Scenario()
        {
            this._acts = new();
            this._taskLoggers = new();
        }

        public Scenario AddParallelPlans(params IUsersPerformancePlan[] performancePlan)
        {
            this.AddActs(ActType.Parallel, performancePlan);

            return this;
        }

        public Scenario AddSequentialPlans(params IUsersPerformancePlan[] performancePlan)
        {
            this.AddActs(ActType.Sequential, performancePlan);

            return this;
        }

        private Scenario AddActs(ActType launchType, params IUsersPerformancePlan[] performancePlan)
        {
            this._acts.Add(new(launchType, performancePlan));

            return this;
        }

        public async Task StartAsync()
        {
            this.StartLoggers();
            ScenarioTimer.Time.Start();

            foreach (var (launchType, plans) in this._acts)
            {
                switch (launchType)
                {
                    case ActType.Parallel:

                        var tasks = new List<Task>();

                        foreach (var plan in plans)
                        {
                            tasks.Add(Task.Run(async () =>
                            {
                                await plan.StartAsync();
                            }));
                        }

                        await Task.WhenAll(tasks.ToArray());

                        break;

                    case ActType.Sequential:

                        foreach (var plan in plans)
                        {
                            await plan.StartAsync();
                        }

                        break;
                }
            }

            ScenarioTimer.Time.Stop();

            await this.WaitLoggersAsync();
        }

        private void StartLoggers()
        {
            foreach (var (_, plans) in this._acts)
            {
                foreach (var plan in plans)
                {
                    if (plan.User.Logger is null)
                    {
                        continue;
                    }

                    Console.WriteLine($"Start Loggers");

                    //var task = Task.Run(() => plan.User.Logger.Start());
                    var task = plan.User.Logger.StartAsync();
                    this._taskLoggers.Add(task);
                }
            }
        }

        private async Task WaitLoggersAsync()
        {
            foreach (var (_, plans) in this._acts)
            {
                foreach (var plan in plans)
                {
                    if (plan.User.Logger is null)
                    {
                        continue;
                    }

                    Console.WriteLine($"Stop Logger");

                    plan.User.Logger.ProcessStop();
                }
            }

            Console.WriteLine($"Wait Logger");

            await Task.WhenAll(_taskLoggers.ToArray());
        }
    }
}
