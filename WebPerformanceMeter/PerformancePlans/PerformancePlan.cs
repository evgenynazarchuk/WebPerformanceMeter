namespace WebPerformanceMeter.PerformancePlans
{
    using System.Threading.Tasks;
    using WebPerformanceMeter.Users;

    public abstract class PerformancePlan
    {
        public readonly User User;

        public PerformancePlan(User user)
        {
            this.User = user;
        }

        public abstract Task StartAsync();
    }
}
