﻿using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;

namespace WebPerformanceMeter.PerformancePlans
{
    public abstract class BasicConstantUsers : UsersPerformancePlan
    {
        public BasicConstantUsers(
            IBasicUser user,
            int usersCount,
            int userLoopCount = 1)
            : base(user)
        {
            this.usersCount = usersCount;
            this.userLoopCount = userLoopCount;
            this.invokedUsers = new Task[usersCount];
        }

        public override async Task StartAsync()
        {
            for (int i = 0; i < this.usersCount; i++)
            {
                this.invokedUsers[i] = this.InvokeUserAsync();
            }

            await Task.WhenAll(this.invokedUsers);
        }

        public abstract Task InvokeUserAsync();

        protected readonly int usersCount;

        protected readonly int userLoopCount;

        protected readonly Task[] invokedUsers;
    }
}
