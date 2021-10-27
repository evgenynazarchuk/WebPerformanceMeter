using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Users;

namespace WebPerformanceMeter
{
    public abstract class UsersPerformancePlan
    {
        public UsersPerformancePlan(IBasicUser user)
        {
            this.User = (BasicUser)user;
        }

        public abstract Task StartAsync();

        public readonly BasicUser User;
    }
}
