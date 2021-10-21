using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Users;

namespace WebPerformanceMeter.PerformancePlans
{
    public sealed class ConstantUsers<TEntity> : ConstantUsersBase
        where TEntity : class
    {
        private readonly IDataReader dataReader;

        private readonly bool reuseDataInLoop;

        public ConstantUsers(
            ITypedUser<TEntity> user,
            int usersCount,
            IDataReader dataReader,
            int userLoopCount = 1,
            bool reuseDataInLoop = true)
            : base(user, usersCount, userLoopCount)
        {
            this.dataReader = dataReader;
            this.reuseDataInLoop = reuseDataInLoop;
        }

        public override Task InvokeUserAsync()
        {
            return ((ITypedUser<TEntity>)this.User).InvokeAsync(this.userLoopCount, this.dataReader, this.reuseDataInLoop);
        }
    }
}
