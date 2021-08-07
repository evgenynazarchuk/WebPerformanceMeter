using System.Threading.Tasks;

namespace WebPerformanceMeter.PerformancePlans
{
    public class PerformancePlan
    {
        public virtual Task StartAsync() { return Task.CompletedTask; }
    }
}
