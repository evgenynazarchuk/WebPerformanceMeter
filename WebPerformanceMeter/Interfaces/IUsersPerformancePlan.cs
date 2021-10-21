using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;

namespace WebPerformanceMeter.PerformancePlans
{
    public interface IUsersPerformancePlan
    {
        Task StartAsync();
    }
}
