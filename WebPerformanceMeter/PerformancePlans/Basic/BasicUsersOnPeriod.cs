using System;
using System.Threading.Tasks;
using System.Timers;
using WebPerformanceMeter.Extensions;
using WebPerformanceMeter.Interfaces;

namespace WebPerformanceMeter.PerformancePlans
{
    public abstract class BasicUsersOnPeriod : UsersPerformancePlan
    {
        public BasicUsersOnPeriod(
            IBasicUser user,
            int totalUsers,
            TimeSpan performancePlanDuration,
            TimeSpan? minimalInvokePeriod = null,
            int userLoopCount = 1)
            : base(user)
        {
            this.totalUsers = totalUsers;
            this.userPerformancePlanDuration = performancePlanDuration;
            this.invokedUsers = new Task[this.totalUsers];
            this.currentInvoke = 0;
            this.minimalInvokePeriod = minimalInvokePeriod ?? 1000.Milliseconds();
            this.CalculateUserCountOnInterval(ref this.usersCount, ref this.interval);

            this.runner = new Timer(this.interval);
            this.runner.Elapsed += (sender, e) => this.InvokeUsers();

            this.userLoopCount = userLoopCount;
        }

        public override async Task StartAsync()
        {
            this.runner.Start();
            await this.WaitPerformancePlanTerminationAsync();
            this.runner.Stop();
            this.runner.Close();

            await this.WaitUserTerminationAsync();
        }

        public void InvokeUsers()
        {
            if (this.currentInvoke == this.totalUsers)
                return;

            for (int i = 0; i < this.usersCount; i++)
            {
                this.invokedUsers[this.currentInvoke] = this.StartUserAsync();
                this.currentInvoke++;
            }
        }

        protected abstract Task StartUserAsync();

        private void CalculateUserCountOnInterval(ref int userCount, ref int interval)
        {
            interval = 1;
            userCount = 1;

            if (this.userPerformancePlanDuration.TotalMilliseconds > this.totalUsers)
            {
                interval = (int)this.userPerformancePlanDuration.TotalMilliseconds / totalUsers;
            }
            else
            {
                userCount = this.totalUsers / (int)this.userPerformancePlanDuration.TotalMilliseconds;
            }

            if (interval < this.minimalInvokePeriod.TotalMilliseconds)
            {
                userCount *= (int)this.minimalInvokePeriod.TotalMilliseconds / this.interval;
                interval = (int)this.minimalInvokePeriod.TotalMilliseconds;
            }
        }

        private async Task WaitUserTerminationAsync()
        {
            foreach (var user in this.invokedUsers)
            {
                if (user is not null)
                {
                    await user;
                }
            }
        }

        private async Task WaitPerformancePlanTerminationAsync()
        {
            await Task.Delay(this.userPerformancePlanDuration + 500.Milliseconds());
        }

        protected readonly int totalUsers;

        protected readonly TimeSpan userPerformancePlanDuration;

        protected readonly Task[] invokedUsers;

        protected readonly int usersCount;

        protected readonly int interval;

        protected readonly Timer runner;

        protected readonly TimeSpan minimalInvokePeriod;

        protected int currentInvoke;

        protected readonly int userLoopCount;
    }
}