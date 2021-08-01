using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebPerformanceMeter.Users;
using WebPerformanceMeter.Interfaces;

namespace WebPerformanceMeter.PerformancePlans
{
    public sealed class ConstantUsers : PerformancePlan
    {
        private readonly User User;

        private readonly int UsersCount;

        private readonly int UserLoopCount;

        private readonly Task[] InvokedUsers;

        private readonly IEntityReader? DataReader;

        private readonly bool ReuseDataInLoop;

        public ConstantUsers(
            User user,
            int usersCount, 
            int userLoopCount = 1,
            IEntityReader? dataReader = null,
            bool reuseDataInLoop = true
            )
        {
            this.User = user;
            this.UsersCount = usersCount;
            this.UserLoopCount = userLoopCount;
            this.InvokedUsers = new Task[UsersCount];
            this.DataReader = dataReader;
            this.ReuseDataInLoop = reuseDataInLoop;
        }

        public override async Task StartAsync()
        {
            for (int i = 0; i < UsersCount; i++)
            {
                this.InvokedUsers[i] = User.InvokeAsync(this.UserLoopCount, this.DataReader, this.ReuseDataInLoop);
            }

            Task.WaitAll(this.InvokedUsers);

            await Task.CompletedTask;
        }
    }
}
