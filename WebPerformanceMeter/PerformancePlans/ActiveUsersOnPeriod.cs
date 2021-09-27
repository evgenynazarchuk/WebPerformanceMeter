using System;
using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Support;
using WebPerformanceMeter.Users;

namespace WebPerformanceMeter.PerformancePlans
{
    public sealed class ActiveUsersOnPeriod : PerformancePlan
    {
        private readonly int _activeUsersCount;

        private readonly TimeSpan _performancePlanDuration;

        private readonly Task[] _activeUsers;

        private readonly int _userLoopCount;

        private readonly IEntityReader? _dataReader;

        private readonly bool _reuseDataInLoop;

        public ActiveUsersOnPeriod(
            User user,
            int activeUsersCount,
            TimeSpan performancePlanDuration,
            int userLoopCount = 1,
            IEntityReader? dataReader = null,
            bool reuseDataInLoop = true)
            : base(user)
        {
            this._activeUsersCount = activeUsersCount;
            this._activeUsers = new Task[this._activeUsersCount];
            this._performancePlanDuration = performancePlanDuration;
            this._userLoopCount = userLoopCount;
            this._dataReader = dataReader;
            this._reuseDataInLoop = reuseDataInLoop;
        }

        public override async Task StartAsync()
        {
            double endTime = ScenarioTimer.Time.Elapsed.TotalSeconds + this._performancePlanDuration.TotalSeconds;

            while (ScenarioTimer.Time.Elapsed.TotalSeconds < endTime)
            {
                for (int i = 0; i < this._activeUsersCount; i++)
                {
                    if (this._activeUsers[i] is null || this._activeUsers[i].IsCompleted)
                    {
                        this._activeUsers[i] = this.User.InvokeAsync(this._userLoopCount, this._dataReader, this._reuseDataInLoop);
                    }
                }
            }

            await this.WaitUserTerminationAsync();
        }

        private async Task WaitUserTerminationAsync()
        {
            foreach (var user in this._activeUsers)
            {
                if (user is not null)
                {
                    await user;
                }
            }
        }
    }
}