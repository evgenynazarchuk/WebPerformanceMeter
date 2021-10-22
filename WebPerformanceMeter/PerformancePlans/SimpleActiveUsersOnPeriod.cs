using System;
using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.PerformancePlans;

namespace WebPerformanceMeter
{
    public sealed class ActiveUsersOnPeriod : BasicActiveUsersOnPeriod
    {
        public ActiveUsersOnPeriod(
            ISimpleUser user,
            int activeUsersCount,
            TimeSpan performancePlanDuration,
            int userLoopCount = 1)
            : base(user,
                  activeUsersCount,
                  performancePlanDuration,
                  userLoopCount)
        { }

        protected override Task InvokeUserAsync()
        {
            return ((ISimpleUser)this.user).InvokeAsync(this.userLoopCount);
        }
    }
}