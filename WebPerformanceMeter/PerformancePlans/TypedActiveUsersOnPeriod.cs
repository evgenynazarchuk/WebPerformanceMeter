using System;
using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.PerformancePlans;

namespace WebPerformanceMeter
{
    public sealed class ActiveUsersOnPeriod<TData> : BasicActiveUsersOnPeriod
        where TData : class
    {
        private readonly IDataReader<TData> dataReader;

        private readonly bool reuseDataInLoop;

        public ActiveUsersOnPeriod(
            ITypedUser<TData> user,
            int activeUsersCount,
            TimeSpan performancePlanDuration,
            int userLoopCount,
            IDataReader<TData> dataReader,
            bool reuseDataInLoop = true)
            : base(user,
                  activeUsersCount,
                  performancePlanDuration,
                  userLoopCount)
        {
            this.dataReader = dataReader;
            this.reuseDataInLoop = reuseDataInLoop;
        }

        protected override Task InvokeUserAsync()
        {
            return ((ITypedUser<TData>)this.user).InvokeAsync(this.dataReader, this.reuseDataInLoop, this.userLoopCount);
        }
    }
}