using System;
using System.Threading.Tasks;
using System.Timers;
using WebPerformanceMeter.Extensions;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Users;

namespace WebPerformanceMeter.PerformancePlans
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class UsersPerPeriod : PerformancePlan
    {
        private readonly int _totalUsersPerPeriod;

        private readonly TimeSpan _perPeriod;

        private readonly TimeSpan _performancePlanDuration;

        private readonly Task[,] _invokedUsers;

        private readonly int _sizePeriodBuffer;

        private readonly Timer _runner;

        private int _currentPeriod;

        private readonly int _userLoopCount;

        private readonly IEntityReader? _dataReader;

        private readonly bool _reuseDataInLoop;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="usersCountPerPeriod"></param>
        /// <param name="performancePlanDuration"></param>
        /// <param name="perPeriod"></param>
        /// <param name="sizePeriodBuffer"></param>
        /// <param name="userLoopCount"></param>
        /// <param name="dataReader"></param>
        /// <param name="reuseDataInLoop"></param>
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
            this._totalUsersPerPeriod = usersCountPerPeriod;
            this._performancePlanDuration = performancePlanDuration;
            this._sizePeriodBuffer = sizePeriodBuffer;
            this._currentPeriod = 0;
            this._invokedUsers = new Task[sizePeriodBuffer, usersCountPerPeriod];
            this._perPeriod = perPeriod is null ? 1.Seconds() : perPeriod.Value;

            this._runner = new Timer(this._perPeriod.TotalMilliseconds);
            this._runner.Elapsed += (sender, e) => this.InvokeUsers();

            this._userLoopCount = userLoopCount;
            this._dataReader = dataReader;
            this._reuseDataInLoop = reuseDataInLoop;
        }

        public override async Task StartAsync()
        {
            this._runner.Start();
            await this.WaitTerminationPerformancePlanAsync();
            this._runner.Stop();
            this._runner.Close();
            await this.WaitUserTerminationAsync();
        }

        private void InvokeUsers()
        {
            for (var i = 0; i < this._totalUsersPerPeriod; i++)
            {
                this._invokedUsers[this._currentPeriod, i] = this.User.InvokeAsync(this._userLoopCount, this._dataReader, this._reuseDataInLoop);
            }

            this.IncrementPeriod();
        }

        private async Task WaitUserTerminationAsync()
        {
            await this._invokedUsers.Wait(this._sizePeriodBuffer, this._totalUsersPerPeriod);
        }

        private async Task WaitTerminationPerformancePlanAsync()
        {
            await Task.Delay(this._performancePlanDuration + 900.Milliseconds());
        }

        private void IncrementPeriod()
        {
            this._currentPeriod++;

            if (this._currentPeriod == this._sizePeriodBuffer)
            {
                this._currentPeriod = 0;
            }
        }
    }
}