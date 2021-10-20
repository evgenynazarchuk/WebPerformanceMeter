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
    public sealed class UsersOnPeriod : PerformancePlan
    {
        private readonly int _totalUsers;

        private readonly TimeSpan _userPerformancePlanDuration;

        private readonly Task[] _invokedUsers;

        private readonly int _usersCount;

        private readonly int _interval;

        private readonly Timer _runner;

        private readonly TimeSpan _minimalInvokePeriod;

        private int _currentInvoke;

        private readonly int _userLoopCount;

        private readonly IDataReader? _dataReader;

        private readonly bool _reuseDataInLoop;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user">Description user</param>
        /// <param name="totalUsers">Total number of users for the entire period</param>
        /// <param name="performancePlanDuration">Duration of the load period</param>
        /// <param name="minimalInvokePeriod">Minimum period for launching a batch of users </param>
        /// <param name="userLoopCount">Number of periods</param>
        /// <param name="dataReader">Description reader</param>
        /// <param name="reuseDataInLoop">Reuse user data</param>
        public UsersOnPeriod(
            User user,
            int totalUsers,
            TimeSpan performancePlanDuration,
            TimeSpan? minimalInvokePeriod = null,
            int userLoopCount = 1,
            IDataReader? dataReader = null,
            bool reuseDataInLoop = true)
            : base(user)
        {
            this._totalUsers = totalUsers;
            this._userPerformancePlanDuration = performancePlanDuration;
            this._invokedUsers = new Task[this._totalUsers];
            this._currentInvoke = 0;
            this._minimalInvokePeriod = minimalInvokePeriod ?? 1000.Milliseconds();
            this.CalculateUserCountOnInterval(ref this._usersCount, ref this._interval);

            this._runner = new Timer(this._interval);
            this._runner.Elapsed += (sender, e) => this.InvokeUsers();

            this._userLoopCount = userLoopCount;
            this._dataReader = dataReader;
            this._reuseDataInLoop = reuseDataInLoop;
        }

        public override async Task StartAsync()
        {
            this._runner.Start();
            await this.WaitPerformancePlanTerminationAsync();
            this._runner.Stop();
            this._runner.Close();

            await this.WaitUserTerminationAsync();
        }

        public void InvokeUsers()
        {
            if (this._currentInvoke == this._totalUsers)
                return;

            for (int i = 0; i < this._usersCount; i++)
            {
                this._invokedUsers[this._currentInvoke] = this.User.InvokeAsync(this._userLoopCount, this._dataReader, this._reuseDataInLoop);
                this._currentInvoke++;
            }
        }

        private void CalculateUserCountOnInterval(ref int userCount, ref int interval)
        {
            interval = 1;
            userCount = 1;

            if (this._userPerformancePlanDuration.TotalMilliseconds > this._totalUsers)
            {
                interval = (int)this._userPerformancePlanDuration.TotalMilliseconds / _totalUsers;
            }
            else
            {
                userCount = this._totalUsers / (int)this._userPerformancePlanDuration.TotalMilliseconds;
            }

            if (interval < this._minimalInvokePeriod.TotalMilliseconds)
            {
                userCount *= (int)this._minimalInvokePeriod.TotalMilliseconds / this._interval;
                interval = (int)this._minimalInvokePeriod.TotalMilliseconds;
            }
        }

        private async Task WaitUserTerminationAsync()
        {
            foreach (var user in this._invokedUsers)
            {
                if (user is not null)
                {
                    await user;
                }
            }
        }

        private async Task WaitPerformancePlanTerminationAsync()
        {
            await Task.Delay(this._userPerformancePlanDuration + 500.Milliseconds());
        }
    }
}