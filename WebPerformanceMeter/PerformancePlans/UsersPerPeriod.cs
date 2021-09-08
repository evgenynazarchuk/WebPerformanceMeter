namespace WebPerformanceMeter.PerformancePlans
{
    using System;
    using System.Threading.Tasks;
    using System.Timers;
    using WebPerformanceMeter.Extensions;
    using WebPerformanceMeter.Interfaces;
    using WebPerformanceMeter.Users;

    public sealed class UsersPerPeriod : PerformancePlan
    {
        ////public readonly User User;

        private readonly int totalUsersPerPeriod;

        private readonly TimeSpan perPeriod;

        private readonly TimeSpan performancePlanDuration;

        private readonly Task[,] invokedUsers;

        private readonly int sizePeriodBuffer;

        private readonly Timer runner;

        private int currentPeriod;

        private readonly int userLoopCount;

        private readonly IEntityReader? dataReader;

        private readonly bool reuseDataInLoop;

        public UsersPerPeriod(
            User user,
            int usersCountPerPeriod,
            TimeSpan performancePlanDuration,
            TimeSpan? perPeriod = null,
            int sizePeriodBuffer = 60,
            int userLoopCount = 1,
            IEntityReader? dataReader = null,
            bool reuseDataInLoop = true)
            : base(user)
        {
            ////this.User = user;
            this.totalUsersPerPeriod = usersCountPerPeriod;
            this.performancePlanDuration = performancePlanDuration;
            this.sizePeriodBuffer = sizePeriodBuffer;
            this.currentPeriod = 0;
            this.invokedUsers = new Task[sizePeriodBuffer, usersCountPerPeriod];
            this.perPeriod = perPeriod is null ? 1.Seconds() : perPeriod.Value;

            this.runner = new Timer(this.perPeriod.TotalMilliseconds);
            this.runner.Elapsed += (sender, e) => this.InvokeUsers();

            this.userLoopCount = userLoopCount;
            this.dataReader = dataReader;
            this.reuseDataInLoop = reuseDataInLoop;
        }

        public override async Task StartAsync()
        {
            this.runner.Start();
            await this.WaitTerminationPerformancePlanAsync();
            this.runner.Stop();
            this.runner.Close();
            await this.WaitUserTerminationAsync();
        }

        private void InvokeUsers()
        {
            for (var i = 0; i < this.totalUsersPerPeriod; i++)
            {
                this.invokedUsers[this.currentPeriod, i] = this.User.InvokeAsync(this.userLoopCount, this.dataReader, this.reuseDataInLoop);
            }

            this.IncrementPeriod();
        }

        private async Task WaitUserTerminationAsync()
        {
            await this.invokedUsers.Wait(this.sizePeriodBuffer, this.totalUsersPerPeriod);
        }

        private async Task WaitTerminationPerformancePlanAsync()
        {
            await Task.Delay(this.performancePlanDuration + 900.Milliseconds());
        }

        private void IncrementPeriod()
        {
            this.currentPeriod++;

            if (this.currentPeriod == this.sizePeriodBuffer)
            {
                this.currentPeriod = 0;
            }
        }
    }
}