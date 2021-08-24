namespace WebPerformanceMeter.PerformancePlans
{
    using System.Threading.Tasks;

    public abstract class PerformancePlan
    {
        public abstract Task StartAsync();
    }
}
