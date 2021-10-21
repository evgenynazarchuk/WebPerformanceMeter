using System;
using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Support;
using WebPerformanceMeter.Users;

namespace WebPerformanceMeter.PerformancePlans
{
    public sealed class ActiveUsersOnPeriod : ActiveUsersOnPeriodBase
    {
        public ActiveUsersOnPeriod(
            IUser user,
            int activeUsersCount,
            TimeSpan performancePlanDuration,
            int userLoopCount = 1)
            : base(user,
                  activeUsersCount,
                  performancePlanDuration,
                  userLoopCount) { }

        protected override Task InvokeUserAsync()
        {
            return ((IUser)this.User).InvokeAsync(this.userLoopCount);
        }
    }
}