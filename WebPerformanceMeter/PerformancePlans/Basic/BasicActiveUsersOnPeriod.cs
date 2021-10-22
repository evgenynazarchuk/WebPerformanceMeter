using System;
using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Support;
using WebPerformanceMeter.Users;

namespace WebPerformanceMeter.PerformancePlans
{
    public abstract class BasicActiveUsersOnPeriod : UsersPerformancePlan
    {
        protected readonly int activeUsersCount;

        protected readonly TimeSpan performancePlanDuration;

        protected readonly Task[] activeUsers;

        protected readonly int userLoopCount;

        public BasicActiveUsersOnPeriod(
            IBaseUser user,
            int activeUsersCount,
            TimeSpan performancePlanDuration,
            int userLoopCount = 1)
            : base(user)
        {
            this.activeUsersCount = activeUsersCount;
            this.activeUsers = new Task[this.activeUsersCount];
            this.performancePlanDuration = performancePlanDuration;
            this.userLoopCount = userLoopCount;
        }

        public override async Task StartAsync()
        {
            double endTime = ScenarioTimer.Time.Elapsed.TotalSeconds + this.performancePlanDuration.TotalSeconds;

            while (ScenarioTimer.Time.Elapsed.TotalSeconds < endTime)
            {
                for (int i = 0; i < this.activeUsersCount; i++)
                {
                    if (this.activeUsers[i] is null || this.activeUsers[i].IsCompleted)
                    {
                        this.activeUsers[i] = this.InvokeUserAsync();
                    }
                }
            }

            await this.WaitUserTerminationAsync();
        }

        protected abstract Task InvokeUserAsync();

        private async Task WaitUserTerminationAsync()
        {
            foreach (var user in this.activeUsers)
            {
                if (user is not null)
                {
                    await user;
                }
            }
        }
    }
}