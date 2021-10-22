using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.PerformancePlans;

namespace WebPerformanceMeter
{
    public sealed class ConstantUsers : BasicConstantUsers
    {
        public ConstantUsers(
            ISimpleUser user,
            int usersCount,
            int userLoopCount = 1)
            : base(user, usersCount, userLoopCount) { }

        public override Task InvokeUserAsync()
        {
            return ((ISimpleUser)this.user).InvokeAsync(this.userLoopCount);
        }
    }
}
