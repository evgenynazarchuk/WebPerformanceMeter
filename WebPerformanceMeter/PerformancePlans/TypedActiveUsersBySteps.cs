using System;
using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.PerformancePlans;

namespace WebPerformanceMeter
{
    public sealed class ActiveUsersBySteps<TData> : BasicActiveUsersBySteps
        where TData : class
    {
        private readonly IDataReader<TData> dataReader;

        private readonly bool reuseDataInLoop;

        public ActiveUsersBySteps(
            ITypedUser<TData> user,
            int fromActiveUsersCount,
            int toActiveUsersCount,
            int usersStep,
            IDataReader<TData> dataReader,
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
            return ((ITypedUser<TData>)this.User).InvokeAsync(this.dataReader, this.reuseDataInLoop, this.userLoopCount);
        }
    }
}