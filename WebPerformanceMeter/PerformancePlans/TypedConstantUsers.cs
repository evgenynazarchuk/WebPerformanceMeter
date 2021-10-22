using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Users;
using WebPerformanceMeter.PerformancePlans;

namespace WebPerformanceMeter
{
    public sealed class ConstantUsers<TData> : BasicConstantUsers
        where TData : class
    {
        private readonly IDataReader<TData> dataReader;

        private readonly bool reuseDataInLoop;

        public ConstantUsers(
            ITypedUser<TData> user,
            int usersCount,
            IDataReader<TData> dataReader,
            int userLoopCount = 1,
            bool reuseDataInLoop = true)
            : base(user, usersCount, userLoopCount)
        {
            this.dataReader = dataReader;
            this.reuseDataInLoop = reuseDataInLoop;
        }

        public override Task InvokeUserAsync()
        {
            return ((ITypedUser<TData>)this.user).InvokeAsync(this.dataReader, this.reuseDataInLoop, this.userLoopCount);
        }
    }
}
