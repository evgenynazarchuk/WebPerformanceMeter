using System;
using System.Threading.Tasks;
using System.Timers;
using WebPerformanceMeter.Extensions;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Users;

namespace WebPerformanceMeter.PerformancePlans
{
    public sealed class UsersPerPeriod : UsersPerPeriodBase
    {
        public UsersPerPeriod(
            IUser user,
            int usersCountPerPeriod,
            TimeSpan performancePlanDuration,
            TimeSpan? perPeriod = null,
            int sizePeriodBuffer = 60,
            int userLoopCount = 1)
            : base(user,
                  usersCountPerPeriod,
                  performancePlanDuration,
                  perPeriod,
                  sizePeriodBuffer,
                  userLoopCount)
        { }

        protected override Task StartUserAsync()
        { 
            return ((IUser)this.User).InvokeAsync(this.userLoopCount);
        }
    }
}