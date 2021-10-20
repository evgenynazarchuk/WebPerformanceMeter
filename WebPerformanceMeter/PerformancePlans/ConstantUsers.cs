using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Users;

namespace WebPerformanceMeter.PerformancePlans
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ConstantUsers : PerformancePlan
    {
        private readonly int _usersCount;

        private readonly int _userLoopCount;

        private readonly Task[] _invokedUsers;

        private readonly IDataReader? _dataReader;

        private readonly bool _reuseDataInLoop;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="usersCount"></param>
        /// <param name="userLoopCount"></param>
        /// <param name="dataReader"></param>
        /// <param name="reuseDataInLoop"></param>
        public ConstantUsers(
            User user,
            int usersCount,
            int userLoopCount = 1,
            IDataReader? dataReader = null,
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
