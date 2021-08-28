using System;
using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Support;
using WebPerformanceMeter.Users;

namespace WebPerformanceMeter.PerformancePlans
{
    public sealed class ActiveUsersOnPeriod : PerformancePlan
    {
        private readonly User PerformanceUser;

        private readonly int ActiveUsersCount;

        private readonly TimeSpan PerformancePlanDuration;

        private readonly Task[] ActiveUsers;

        private readonly int UserLoopCount;

        private readonly IEntityReader? DataReader;

        private readonly bool ReuseDataInLoop;

        public ActiveUsersOnPeriod(
            User user,
            int activeUsersCount,
            TimeSpan performancePlanDuration,
            int userLoopCount = 1,
            IEntityReader? dataReader = null,
            bool reuseDataInLoop = true)
        {
            this.PerformanceUser = user;
            this.ActiveUsersCount = activeUsersCount;
            this.ActiveUsers = new Task[this.ActiveUsersCount];
            this.PerformancePlanDuration = performancePlanDuration;
            this.UserLoopCount = userLoopCount;
            this.DataReader = dataReader;
            this.ReuseDataInLoop = reuseDataInLoop;
        }

        public override async Task StartAsync()
        {
            double endTime = Scenario.ScenarioWatchTime.Elapsed.TotalSeconds + this.PerformancePlanDuration.TotalSeconds;
            while (Scenario.ScenarioWatchTime.Elapsed.TotalSeconds < endTime)
            {
                for (int i = 0; i < this.ActiveUsersCount; i++)
                {
                    if (this.ActiveUsers[i] is null || this.ActiveUsers[i].IsCompleted)
                    {
                        this.ActiveUsers[i] = this.PerformanceUser.InvokeAsync(this.UserLoopCount, this.DataReader, this.ReuseDataInLoop);
                    }
                }
            }
            await this.WaitUserTerminationAsync();
        }

        private async Task WaitUserTerminationAsync()
        {
            foreach (var user in this.ActiveUsers)
            {
                if (user is not null)
                {
                    await user;
                }
            }
        }
    }
}