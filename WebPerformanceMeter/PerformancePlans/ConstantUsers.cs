namespace WebPerformanceMeter.PerformancePlans
{
    using System.Threading.Tasks;
    using WebPerformanceMeter.Interfaces;
    using WebPerformanceMeter.Users;

    public sealed class ConstantUsers : PerformancePlan
    {
        private readonly int usersCount;

        private readonly int userLoopCount;

        private readonly Task[] invokedUsers;

        private readonly IEntityReader? dataReader;

        private readonly bool reuseDataInLoop;

        public ConstantUsers(
            User user,
            int usersCount,
            int userLoopCount = 1,
            IEntityReader? dataReader = null,
            bool reuseDataInLoop = true)
            : base(user)
        {
            this.usersCount = usersCount;
            this.userLoopCount = userLoopCount;
            this.invokedUsers = new Task[this.usersCount];
            this.dataReader = dataReader;
            this.reuseDataInLoop = reuseDataInLoop;
        }

        public override async Task StartAsync()
        {
            for (int i = 0; i < this.usersCount; i++)
            {
                this.invokedUsers[i] = this.User.InvokeAsync(this.userLoopCount, this.dataReader, this.reuseDataInLoop);
            }

            Task.WaitAll(this.invokedUsers);

            await Task.CompletedTask;
        }
    }
}
