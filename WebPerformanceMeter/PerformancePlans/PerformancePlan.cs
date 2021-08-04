using System.Threading.Tasks;

namespace WebPerformanceMeter.PerformancePlans
{
    public abstract class PerformancePlan
    {
        public abstract Task StartAsync();
    }
}
