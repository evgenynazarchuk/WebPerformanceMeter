using System;
using System.Threading.Tasks;
using System.Timers;
using WebPerformanceMeter.Extensions;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Users;

namespace WebPerformanceMeter.PerformancePlans
{
    public sealed class UsersPerPeriod : PerformancePlan
    {
        private readonly User User;

        private readonly int TotalUsersPerPeriod;

        private readonly TimeSpan PerPeriod;

        private readonly TimeSpan PerformancePlanDuration;

        private readonly Task[,] InvokedUsers;

        private readonly int SizePeriodBuffer;

        private readonly Timer Runner;

        private int CurrentPeriod;

        private readonly int UserLoopCount;

        private readonly IEntityReader? DataReader;

        private readonly bool ReuseDataInLoop;

        public UsersPerPeriod(
            User user,
            int usersCountPerPeriod,
            TimeSpan performancePlanDuration,
            TimeSpan? perPeriod = null,
            int sizePeriodBuffer = 60,
            int userLoopCount = 1,
            IEntityReader? dataReader = null,
            bool reuseDataInLoop = true)
        {
            this.User = user;
            this.TotalUsersPerPeriod = usersCountPerPeriod;
            this.PerformancePlanDuration = performancePlanDuration;
            this.SizePeriodBuffer = sizePeriodBuffer;
            this.CurrentPeriod = 0;
            this.InvokedUsers = new Task[sizePeriodBuffer, usersCountPerPeriod];
            this.PerPeriod = perPeriod is null ? 1.Seconds() : perPeriod.Value;
            this.Runner = new Timer(this.PerPeriod.TotalMilliseconds);
            this.Runner.Elapsed += (sender, e) => InvokeUsers();
            this.UserLoopCount = userLoopCount;
            this.DataReader = dataReader;
            this.ReuseDataInLoop = reuseDataInLoop;
        }

        public override async Task StartAsync()
        {
            this.Runner.Start();
            await this.WaitTerminationPerformancePlanAsync();
            this.Runner.Stop();
            this.Runner.Close();
            await this.WaitUserTerminationAsync();
        }

        private void InvokeUsers()
        {
            for (var i = 0; i < this.TotalUsersPerPeriod; i++)
            {
                this.InvokedUsers[this.CurrentPeriod, i] = this.User.InvokeAsync();
            }

            this.IncrementPeriod();
        }

        private async Task WaitUserTerminationAsync()
        {
            await this.InvokedUsers.Wait(this.SizePeriodBuffer, this.TotalUsersPerPeriod);
        }

        private async Task WaitTerminationPerformancePlanAsync()
        {
            await Task.Delay(this.PerformancePlanDuration + 900.Milliseconds());
        }

        private void IncrementPeriod()
        {
            this.CurrentPeriod++;

            if (this.CurrentPeriod == this.SizePeriodBuffer)
            {
                this.CurrentPeriod = 0;
            }
        }
    }
}