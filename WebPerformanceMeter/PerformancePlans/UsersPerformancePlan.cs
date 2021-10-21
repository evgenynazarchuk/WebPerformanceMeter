using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;

namespace WebPerformanceMeter.PerformancePlans
{
    public abstract class UsersPerformancePlan : IUsersPerformancePlan
    {
        public readonly IBaseUser User;

        public UsersPerformancePlan(IBaseUser user)
        {
            this.User = user;
        }

        public abstract Task StartAsync();
    }
}
