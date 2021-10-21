using System;
using System.Threading.Tasks;
using System.Timers;
using WebPerformanceMeter.Extensions;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Users;

namespace WebPerformanceMeter.PerformancePlans
{
    public sealed class UsersOnPeriod<TEntity> : UsersOnPeriodBase
        where TEntity : class
    {
        private readonly IDataReader dataReader;

        private readonly bool reuseDataInLoop;

        public UsersOnPeriod(
            ITypedUser<TEntity> user,
            int totalUsers,
            TimeSpan performancePlanDuration,
            IDataReader dataReader,
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
            return ((ITypedUser<TEntity>)this.User).InvokeAsync(this.userLoopCount, this.dataReader, this.reuseDataInLoop);
        }
    }
}