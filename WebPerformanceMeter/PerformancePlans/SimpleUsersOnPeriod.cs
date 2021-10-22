﻿using System;
using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.PerformancePlans;

namespace WebPerformanceMeter
{
    public sealed class UsersOnPeriod : BasicUsersOnPeriod
    {
        public UsersOnPeriod(
            ISimpleUser user,
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
            return ((ISimpleUser)this.user).InvokeAsync(this.userLoopCount);
        }
    }
}