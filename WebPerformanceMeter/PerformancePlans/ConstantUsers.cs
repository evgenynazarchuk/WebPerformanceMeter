using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Users;

namespace WebPerformanceMeter.PerformancePlans
{
    public sealed class ConstantUsers : PerformancePlan
    {
        private readonly PerformanceUser PerformanceUser;

        private readonly int UsersCount;

        private readonly int UserLoopCount;

        private readonly Task[] InvokedUsers;

        private readonly IEntityReader? DataReader;

        private readonly bool ReuseDataInLoop;

        public ConstantUsers(
            PerformanceUser user,
            int usersCount,
            int userLoopCount = 1,
            IEntityReader? dataReader = null,
            bool reuseDataInLoop = true
            )
        {
            this.PerformanceUser = user;
            this.UsersCount = usersCount;
            this.UserLoopCount = userLoopCount;
            this.InvokedUsers = new Task[this.UsersCount];
            this.DataReader = dataReader;
            this.ReuseDataInLoop = reuseDataInLoop;
        }

        public override async Task StartAsync()
        {
            for (int i = 0; i < UsersCount; i++)
            {
                this.InvokedUsers[i] = this.PerformanceUser.InvokeAsync(this.UserLoopCount, this.DataReader, this.ReuseDataInLoop);
            }

            Task.WaitAll(this.InvokedUsers);

            await Task.CompletedTask;
        }
    }
}
