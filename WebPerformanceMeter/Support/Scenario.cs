using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebPerformanceMeter.PerformancePlans;
using WebPerformanceMeter.Users;
using WebPerformanceMeter.Reports;

namespace WebPerformanceMeter.Support
{
    public sealed class Scenario
    {
        public Scenario(string projectName = "", string testRunId = "")
        {
            this.ProjectName = projectName;
            this.TestRunId = testRunId;
            this._acts = new();
            this._reports = new();
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

        public async Task Start()
        {
            this.StartWatcher();

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

            await this.StopAndWaitWatcher();
        }

        private Scenario AddActs(ActType launchType, params UsersPerformancePlan[] performancePlan)
        {
            this._acts.Add(new(launchType, performancePlan));

            return this;
        }

        private void InitDefaultReport()
        {
            foreach (var (_, plans) in this._acts)
            {
                foreach (var plan in plans)
                {
                    if (plan.User is BasicHttpUser)
                        plan.User.Watcher.AddReport(HttpReportFileSingleton.GetInstance(this.ProjectName, this.TestRunId));

                    if (plan.User is BasicGrpcUser)
                        plan.User.Watcher.AddReport(GrpcReportFileSingleton.GetInstance(this.ProjectName, this.TestRunId));

                    if (plan.User is BasicWebSocketUser)
                        plan.User.Watcher.AddReport(WebSocketReportFileSingleton.GetInstance(this.ProjectName, this.TestRunId));

                    if (plan.User is BasicChromiumUser)
                        plan.User.Watcher.AddReport(ChromiumReportFileSingleton.GetInstance(this.ProjectName, this.TestRunId));
                }
            }
        }

        private void StartWatcher()
        {
            this.InitDefaultReport();

            foreach (var (_, plans) in this._acts)
            {
                foreach (var plan in plans)
                {
                    Console.WriteLine($"Info: Start Reports");

                    var task = plan.User.Watcher.StartAsync();
                    this._reports.AddRange(task);
                }
            }
        }

        public readonly string ProjectName;

        public readonly string TestRunId;

        private async Task StopAndWaitWatcher()
        {
            Console.WriteLine($"Info: Stop Reports");

            foreach (var (_, plans) in this._acts)
            {
                foreach (var plan in plans)
                {
                    plan.User.Watcher.Stop();
                }
            }

            Console.WriteLine($"Info: Wait Reports");

            await Task.WhenAll(this._reports.ToArray());
        }

        private readonly List<KeyValuePair<ActType, UsersPerformancePlan[]>> _acts;

        private readonly List<Task> _reports;
    }
}
