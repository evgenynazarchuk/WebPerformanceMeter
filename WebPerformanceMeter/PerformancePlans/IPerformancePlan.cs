using System.Threading.Tasks;

namespace WebPerformanceMeter.PerformancePlans
{
    public interface IPerformancePlan
    {
        Task StartAsync();
    }
}
