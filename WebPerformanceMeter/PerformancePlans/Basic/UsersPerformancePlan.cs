using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.PerformancePlans;
using WebPerformanceMeter.Users;

namespace WebPerformanceMeter
{
    public abstract class UsersPerformancePlan : IUsersPerformancePlan
    {
        public UsersPerformancePlan(IBasicUser user)
        {
            this.User = (BasicUser)user;
        }

        public abstract Task StartAsync();

        public readonly BasicUser User;
    }
}
