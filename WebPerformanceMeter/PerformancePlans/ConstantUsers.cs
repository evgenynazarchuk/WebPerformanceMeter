using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Users;

namespace WebPerformanceMeter.PerformancePlans
{
    public sealed class ConstantUsers : PerformancePlan
    {
        private readonly int _usersCount;

        private readonly int _userLoopCount;

        private readonly Task[] _invokedUsers;

        private readonly IEntityReader? _dataReader;

        private readonly bool _reuseDataInLoop;

        public ConstantUsers(
            User user,
            int usersCount,
            int userLoopCount = 1,
            IEntityReader? dataReader = null,
            bool reuseDataInLoop = true)
            : base(user)
        {
            this._usersCount = usersCount;
            this._userLoopCount = userLoopCount;
            this._invokedUsers = new Task[this._usersCount];
            this._dataReader = dataReader;
            this._reuseDataInLoop = reuseDataInLoop;
        }

        public override async Task StartAsync()
        {
            for (int i = 0; i < this._usersCount; i++)
            {
                this._invokedUsers[i] = this.User.InvokeAsync(this._userLoopCount, this._dataReader, this._reuseDataInLoop);
            }

            Task.WaitAll(this._invokedUsers);

            await Task.CompletedTask;
        }
    }
}
