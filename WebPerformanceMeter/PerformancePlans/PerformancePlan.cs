using System.Threading.Tasks;
using WebPerformanceMeter.Users;

namespace WebPerformanceMeter.PerformancePlans
{
    public abstract class PerformancePlan : IPerformancePlan
    {
        public readonly User User;

        public PerformancePlan(User user)
        {
            this.User = user;
        }

        public abstract Task StartAsync();
    }
}
