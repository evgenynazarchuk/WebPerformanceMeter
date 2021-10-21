using System;
using System.Threading.Tasks;
using System.Timers;
using WebPerformanceMeter.Extensions;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Users;

namespace WebPerformanceMeter.PerformancePlans
{
    public sealed class UsersPerPeriod<TEntity> : UsersPerPeriodBase
        where TEntity : class
    {
        private readonly IDataReader? _dataReader;

        private readonly bool _reuseDataInLoop;

        public UsersPerPeriod(
            ITypedUser<TEntity> user,
            int usersCountPerPeriod,
            TimeSpan performancePlanDuration,
            IDataReader dataReader,
            TimeSpan? perPeriod = null,
            int sizePeriodBuffer = 60,
            int userLoopCount = 1,
            bool reuseDataInLoop = true)
            : base(user,
                  usersCountPerPeriod,
                  performancePlanDuration,
                  perPeriod,
                  sizePeriodBuffer,
                  userLoopCount)
        {
            this._dataReader = dataReader;
            this._reuseDataInLoop = reuseDataInLoop;
        }

        protected override Task StartUserAsync()
        {
            return ((ITypedUser<TEntity>)this.User).InvokeAsync(this.userLoopCount, this._dataReader, this._reuseDataInLoop);
        }
    }
}