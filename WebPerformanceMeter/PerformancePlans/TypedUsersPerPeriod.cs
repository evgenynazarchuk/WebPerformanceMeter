using System;
using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.PerformancePlans;

namespace WebPerformanceMeter
{
    public sealed class UsersPerPeriod<TData> : BasicUsersPerPeriod
        where TData : class
    {
        private readonly IDataReader<TData> _dataReader;

        private readonly bool _reuseDataInLoop;

        public UsersPerPeriod(
            ITypedUser<TData> user,
            int usersCountPerPeriod,
            TimeSpan performancePlanDuration,
            IDataReader<TData> dataReader,
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
            return ((ITypedUser<TData>)this.user).InvokeAsync(this._dataReader, this._reuseDataInLoop, this.userLoopCount);
        }
    }
}