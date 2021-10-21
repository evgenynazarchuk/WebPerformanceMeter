using System;
using System.Threading.Tasks;
using System.Timers;
using WebPerformanceMeter.Extensions;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Users;

namespace WebPerformanceMeter.PerformancePlans
{
    public abstract class UsersPerPeriodBase : UsersPerformancePlan
    {
        protected readonly int totalUsersPerPeriod;

        protected readonly TimeSpan perPeriod;

        protected readonly TimeSpan performancePlanDuration;

        protected readonly Task[,] invokedUsers;

        protected readonly int sizePeriodBuffer;

        protected readonly Timer runner;

        protected int currentPeriod;

        protected readonly int userLoopCount;

        public UsersPerPeriodBase(
            IBaseUser user,
            int usersCountPerPeriod,
            TimeSpan performancePlanDuration,
            TimeSpan? perPeriod = null,
            int sizePeriodBuffer = 60,
            int userLoopCount = 1)
            : base(user)
        {
            this.totalUsersPerPeriod = usersCountPerPeriod;
            this.performancePlanDuration = performancePlanDuration;
            this.sizePeriodBuffer = sizePeriodBuffer;
            this.currentPeriod = 0;
            this.invokedUsers = new Task[sizePeriodBuffer, usersCountPerPeriod];
            this.perPeriod = perPeriod is null ? 1.Seconds() : perPeriod.Value;

            this.runner = new Timer(this.perPeriod.TotalMilliseconds);
            this.runner.Elapsed += (sender, e) => this.InvokeUsers();

            this.userLoopCount = userLoopCount;
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
                this.invokedUsers[this.currentPeriod, i] = this.StartUserAsync();
            }

            this.IncrementPeriod();
        }

        protected abstract Task StartUserAsync();

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