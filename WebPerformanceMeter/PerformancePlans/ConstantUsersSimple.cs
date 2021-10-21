using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Users;

namespace WebPerformanceMeter.PerformancePlans
{
    public sealed class ConstantUsers : ConstantUsersBase
    {
        public ConstantUsers(
            IUser user,
            int usersCount,
            int userLoopCount = 1)
            : base(user, usersCount, userLoopCount) { }

        public override Task InvokeUserAsync()
        { 
            return ((IUser)this.User).InvokeAsync(this.userLoopCount);
        }
    }
}
