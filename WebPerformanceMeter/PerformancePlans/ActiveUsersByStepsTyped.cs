using System;
using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Support;
using WebPerformanceMeter.Users;

namespace WebPerformanceMeter.PerformancePlans
{
    public sealed class ActiveUsersBySteps<TEntity> : ActiveUsersByStepsBase
        where TEntity : class
    {
        private readonly IDataReader dataReader;

        private readonly bool reuseDataInLoop;

        public ActiveUsersBySteps(
            ITypedUser<TEntity> user,
            int fromActiveUsersCount,
            int toActiveUsersCount,
            int usersStep,
            IDataReader dataReader,
            TimeSpan? stepPeriodDuration = null,
            TimeSpan? performancePlanDuration = null,
            int userLoopCount = 1,
            bool reuseDataInLoop = true)
            : base(user, 
                  fromActiveUsersCount, 
                  toActiveUsersCount, 
                  usersStep,
                  stepPeriodDuration,
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