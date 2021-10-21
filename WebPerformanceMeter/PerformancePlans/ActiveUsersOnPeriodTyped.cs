using System;
using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;

namespace WebPerformanceMeter.PerformancePlans
{
    public sealed class ActiveUsersOnPeriod<TEntity> : ActiveUsersOnPeriodBase
        where TEntity : class
    {
        private readonly IDataReader dataReader;

        private readonly bool reuseDataInLoop;

        public ActiveUsersOnPeriod(
            ITypedUser<TEntity> user,
            int activeUsersCount,
            TimeSpan performancePlanDuration,
            int userLoopCount,
            IDataReader dataReader,
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
            return ((ITypedUser<TEntity>)this.User).InvokeAsync(this.userLoopCount, this.dataReader, this.reuseDataInLoop);
        }
    }
}