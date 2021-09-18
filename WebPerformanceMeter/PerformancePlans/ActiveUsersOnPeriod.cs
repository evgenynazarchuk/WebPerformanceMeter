namespace WebPerformanceMeter.PerformancePlans
{
    using System;
    using System.Threading.Tasks;
    using WebPerformanceMeter.Interfaces;
    using WebPerformanceMeter.Support;
    using WebPerformanceMeter.Users;

    public sealed class ActiveUsersOnPeriod : PerformancePlan
    {
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
            : base(user)
        {
            this.ActiveUsersCount = activeUsersCount;
            this.ActiveUsers = new Task[this.ActiveUsersCount];
            this.PerformancePlanDuration = performancePlanDuration;
            this.UserLoopCount = userLoopCount;
            this.DataReader = dataReader;
            this.ReuseDataInLoop = reuseDataInLoop;
        }

        public override async Task StartAsync()
        {
            double endTime = ScenarioTimer.Time.Elapsed.TotalSeconds + this.PerformancePlanDuration.TotalSeconds;

            while (ScenarioTimer.Time.Elapsed.TotalSeconds < endTime)
            {
                for (int i = 0; i < this.ActiveUsersCount; i++)
                {
                    if (this.ActiveUsers[i] is null || this.ActiveUsers[i].IsCompleted)
                    {
                        this.ActiveUsers[i] = this.User.InvokeAsync(this.UserLoopCount, this.DataReader, this.ReuseDataInLoop);
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