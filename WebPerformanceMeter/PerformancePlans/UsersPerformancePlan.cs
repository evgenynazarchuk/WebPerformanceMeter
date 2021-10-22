using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.PerformancePlans;

namespace WebPerformanceMeter
{
    public abstract class UsersPerformancePlan : IUsersPerformancePlan
    {
        protected readonly IBaseUser user;

        public IBaseUser User { get => this.user; }

        public UsersPerformancePlan(IBaseUser user)
        {
            this.user = user;
        }

        public abstract Task StartAsync();
    }
}
