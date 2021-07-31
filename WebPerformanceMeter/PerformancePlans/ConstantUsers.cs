using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebPerformanceMeter.Users;
using WebPerformanceMeter.Interfaces;

namespace WebPerformanceMeter.PerformancePlans
{
    public class ConstantUsers : PerformancePlan
    {
        protected readonly User User;

        protected readonly int UsersCount;

        protected readonly int UserLoopCount;

        protected readonly Task[] InvokedUsers;

        protected readonly IEntityReader? DataReader;

        protected readonly bool ReuseDataInLoop;

        public ConstantUsers(
            User user,
            int usersCount, 
            int userLoopCount = 1,
            IEntityReader? dataReader = null,
            bool reuseDataInLoop = true
            )
        {
            User = user;
            UsersCount = usersCount;
            UserLoopCount = userLoopCount;
            InvokedUsers = new Task[UsersCount];
            DataReader = dataReader;
            ReuseDataInLoop = reuseDataInLoop;
        }

        public override async Task StartAsync()
        {
            for (int i = 0; i < UsersCount; i++)
            {
                InvokedUsers[i] = User.InvokeAsync(UserLoopCount, DataReader, ReuseDataInLoop);
            }

            Task.WaitAll(InvokedUsers);

            await Task.CompletedTask;
        }
    }
}
