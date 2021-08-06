using System;
using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Users;

namespace WebPerformanceMeter.PerformancePlans
{
    public sealed class ActiveUsersOnPeriod : PerformancePlan
    {
        private readonly User User;

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
            this.User = user;
            this.ActiveUsersCount = activeUsersCount;
            this.ActiveUsers = new Task[this.ActiveUsersCount];
            this.PerformancePlanDuration = performancePlanDuration;
            this.UserLoopCount = userLoopCount;
            this.DataReader = dataReader;
            this.ReuseDataInLoop = reuseDataInLoop;
        }

        public override async Task StartAsync()
        {
            DateTime plannedPerformancePlanEndTime = DateTime.Now + this.PerformancePlanDuration;

            while (DateTime.Now.CompareTo(plannedPerformancePlanEndTime) < 0)
            {
                for (int i = 0; i < this.ActiveUsersCount; i++)
                {
                    if (this.ActiveUsers[i] is null 
                        || this.ActiveUsers[i].IsCompleted 
                        || this.ActiveUsers[i].IsCanceled 
                        || this.ActiveUsers[i].IsFaulted
                        || this.ActiveUsers[i].IsCompletedSuccessfully)
                    {
                        this.ActiveUsers[i] = this.User.InvokeAsync(this.UserLoopCount, this.DataReader, this.ReuseDataInLoop);
                    }
                }

                // TODO: read from configuration
                //await Task.Delay(50);
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