using System.Collections.Generic;
using System.Threading.Tasks;
using WebPerformanceMeter.PerformancePlans;
using System;

namespace WebPerformanceMeter.Support
{
    public sealed class Scenario
    {
        private readonly List<KeyValuePair<ActType, PerformancePlan[]>> _acts;

        private readonly List<Task> _loggers;

        public Scenario()
        {
            this._acts = new();
            this._loggers = new();
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
            this._acts.Add(new(launchType, performancePlan));

            return this;
        }

        public async Task StartAsync()
        {
            this.StartLoggers();

            ScenarioTimer.Time.Start();

            // TODO: (launchType, plans)
            foreach (var (launchType, plans) in this._acts)
            //foreach (var act in this.acts)
            {
                //var launchType = act.Key;
                //var plans = act.Value;

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

                            // TODO: check
                            //tasks.Add(plan.StartAsync());
                        }

                        Task.WaitAll(tasks.ToArray());

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

                //if (launchType == ActType.Sequential)
                //{
                //    foreach (var plan in plans)
                //    {
                //        await plan.StartPerformanceAsync();
                //    }
                //}
                //else if (launchType == ActType.Parallel)
                //{
                //    var plansWaiter = new List<Task>();
                //
                //    foreach (var plan in plans)
                //    {
                //        plansWaiter.Add(Task.Run(async () =>
                //        {
                //            await plan.StartPerformanceAsync();
                //        }));
                //    }
                //
                //    Task.WaitAll(plansWaiter.ToArray());
                //}
            }

            ScenarioTimer.Time.Stop();

            this.WaitLoggers();
        }

        private void StartLoggers()
        {
            foreach (var (_, plans) in this._acts)
            {
                foreach (var plan in plans)
                {
                    var task = Task.Run(() => plan.User.Logger.Start());
                    this._loggers.Add(task);

                    // TODO: check
                    //var task2 = plan.User.Logger.Start();
                    //_loggers.Add(task2);
                }
            }
        }

        private void WaitLoggers()
        {
            foreach (var (_, plans) in this._acts)
            {
                foreach (var plan in plans)
                {
                    plan.User.Logger.ProcessStop();
                }
            }

            Task.WaitAll(_loggers.ToArray());
        }
    }
}
