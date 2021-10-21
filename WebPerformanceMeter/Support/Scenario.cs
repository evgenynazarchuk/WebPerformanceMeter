using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebPerformanceMeter.PerformancePlans;

namespace WebPerformanceMeter.Support
{
    public sealed class Scenario
    {
        private readonly List<KeyValuePair<ActType, UsersPerformancePlan[]>> _acts;

        private readonly List<Task> _loggers;

        public Scenario()
        {
            this._acts = new();
            this._loggers = new();
        }

        public Scenario AddParallelPlans(params UsersPerformancePlan[] performancePlan)
        {
            this.AddActs(ActType.Parallel, performancePlan);

            return this;
        }

        public Scenario AddSequentialPlans(params UsersPerformancePlan[] performancePlan)
        {
            this.AddActs(ActType.Sequential, performancePlan);

            return this;
        }

        private Scenario AddActs(ActType launchType, params UsersPerformancePlan[] performancePlan)
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

                    default:
                        throw new ApplicationException("Not Implement ActType");
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
                    
                    //var task = Task.Run(() => plan.User.Logger.Start());
                    var task = plan.User.Logger.StartAsync();
                    this._loggers.Add(task);
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

                    plan.User.Logger.ProcessStop();
                }
            }

            await Task.WhenAll(_loggers.ToArray());
        }
    }
}
