using System;
using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.PerformancePlans;

namespace WebPerformanceMeter
{
    public sealed class ActiveUsersBySteps : BasicActiveUsersBySteps
    {
        public ActiveUsersBySteps(
            ISimpleUser user,
            int fromActiveUsersCount,
            int toActiveUsersCount,
            int usersStep,
            TimeSpan? stepPeriodDuration = null,
            TimeSpan? performancePlanDuration = null,
            int userLoopCount = 1)
            : base(user,
                  fromActiveUsersCount,
                  toActiveUsersCount,
                  usersStep,
                  stepPeriodDuration,
                  performancePlanDuration,
                  userLoopCount)
        { }

        protected override Task InvokeUserAsync()
        {
            return ((ISimpleUser)this.User).InvokeAsync(this.userLoopCount);
        }
    }
}