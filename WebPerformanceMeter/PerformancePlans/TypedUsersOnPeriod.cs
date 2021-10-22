using System;
using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.PerformancePlans;

namespace WebPerformanceMeter
{
    public sealed class UsersOnPeriod<TData> : BasicUsersOnPeriod
        where TData : class
    {
        private readonly IDataReader<TData> dataReader;

        private readonly bool reuseDataInLoop;

        public UsersOnPeriod(
            ITypedUser<TData> user,
            int totalUsers,
            TimeSpan performancePlanDuration,
            IDataReader<TData> dataReader,
            TimeSpan? minimalInvokePeriod = null,
            int userLoopCount = 1,
            bool reuseDataInLoop = true)
            : base(user, totalUsers, performancePlanDuration, minimalInvokePeriod, userLoopCount)
        {
            this.dataReader = dataReader;
            this.reuseDataInLoop = reuseDataInLoop;
        }

        protected override Task StartUserAsync()
        {
            return ((ITypedUser<TData>)this.user).InvokeAsync(this.dataReader, this.reuseDataInLoop, this.userLoopCount);
        }
    }
}