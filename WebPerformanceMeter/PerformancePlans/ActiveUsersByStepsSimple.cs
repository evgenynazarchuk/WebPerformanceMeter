using System;
using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Support;
using WebPerformanceMeter.Users;

namespace WebPerformanceMeter.PerformancePlans
{
    public sealed class ActiveUsersBySteps : ActiveUsersByStepsBase
    {
        public ActiveUsersBySteps(
            IUser user,
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
                  userLoopCount) { }

        protected override Task InvokeUserAsync()
        {
            return ((IUser)this.User).InvokeAsync(this.userLoopCount);
        }
    }
}