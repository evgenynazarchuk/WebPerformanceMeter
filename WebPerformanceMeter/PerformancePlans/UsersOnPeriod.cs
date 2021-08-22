using System;
using System.Threading.Tasks;
using System.Timers;
using WebPerformanceMeter.Extensions;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Users;

namespace WebPerformanceMeter.PerformancePlans
{
    public sealed class UsersOnPeriod : PerformancePlan
    {
        private readonly int TotalUsers;

        private readonly TimeSpan UserPerformancePlanDuration;

        private readonly Task[] InvokedUsers;

        private readonly int UsersCount;

        private readonly int Interval;

        private readonly Timer Runner;

        private readonly PerformanceUser PerformanceUser;

        private readonly TimeSpan MinimalInvokePeriod;

        private int CurrentInvoke;

        private readonly int UserLoopCount;

        private readonly IEntityReader? DataReader;

        private readonly bool ReuseDataInLoop;

        public UsersOnPeriod(
            PerformanceUser user,
            int totalUsers,
            TimeSpan performancePlanDuration,
            TimeSpan? minimalInvokePeriod = null,
            int userLoopCount = 1,
            IEntityReader? dataReader = null,
            bool reuseDataInLoop = true)
        {
            this.PerformanceUser = user;
            this.TotalUsers = totalUsers;
            this.UserPerformancePlanDuration = performancePlanDuration;
            this.InvokedUsers = new Task[this.TotalUsers];
            this.CurrentInvoke = 0;
            this.MinimalInvokePeriod = minimalInvokePeriod ?? 1000.Milliseconds();
            this.CalculateUserCountOnInterval(ref this.UsersCount, ref this.Interval);

            this.Runner = new Timer(this.Interval);
            this.Runner.Elapsed += (sender, e) => this.InvokeUsers();

            this.UserLoopCount = userLoopCount;
            this.DataReader = dataReader;
            this.ReuseDataInLoop = reuseDataInLoop;
        }

        public override async Task StartAsync()
        {
            this.Runner.Start();
            await this.WaitPerformancePlanTerminationAsync();
            this.Runner.Stop();
            this.Runner.Close();
            await this.WaitUserTerminationAsync();
        }

        public void InvokeUsers()
        {
            if (this.CurrentInvoke == this.TotalUsers)
                return;

            for (int i = 0; i < this.UsersCount; i++)
            {
                this.InvokedUsers[this.CurrentInvoke] = this.PerformanceUser.InvokeAsync(this.UserLoopCount, this.DataReader, this.ReuseDataInLoop);
                this.CurrentInvoke++;
            }
        }

        private void CalculateUserCountOnInterval(ref int userCount, ref int interval)
        {
            interval = 1;
            userCount = 1;

            if (this.UserPerformancePlanDuration.TotalMilliseconds > this.TotalUsers)
            {
                interval = (int)this.UserPerformancePlanDuration.TotalMilliseconds / TotalUsers;
            }
            else
            {
                userCount = this.TotalUsers / (int)this.UserPerformancePlanDuration.TotalMilliseconds;
            }

            if (interval < this.MinimalInvokePeriod.TotalMilliseconds)
            {
                userCount *= (int)this.MinimalInvokePeriod.TotalMilliseconds / this.Interval;
                interval = (int)this.MinimalInvokePeriod.TotalMilliseconds;
            }
        }

        private async Task WaitUserTerminationAsync()
        {
            foreach (var user in this.InvokedUsers)
            {
                if (user is not null)
                {
                    await user;
                }
            }
        }

        private async Task WaitPerformancePlanTerminationAsync()
        {
            await Task.Delay(this.UserPerformancePlanDuration + 500.Milliseconds());
        }
    }
}