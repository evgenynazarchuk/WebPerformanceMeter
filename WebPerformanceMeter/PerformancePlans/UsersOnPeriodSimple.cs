using System;
using System.Threading.Tasks;
using System.Timers;
using WebPerformanceMeter.Extensions;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Users;

namespace WebPerformanceMeter.PerformancePlans
{
    public sealed class UsersOnPeriod : UsersOnPeriodBase
    {
        public UsersOnPeriod(
            IUser user,
            int totalUsers,
            TimeSpan performancePlanDuration,
            TimeSpan? minimalInvokePeriod = null,
            int userLoopCount = 1)
            : base(user,
                  totalUsers,
                  performancePlanDuration,
                  minimalInvokePeriod,
                  userLoopCount)
        { }


        protected override Task StartUserAsync()
        {
            return ((IUser)this.User).InvokeAsync(this.userLoopCount);
        }
    }
}