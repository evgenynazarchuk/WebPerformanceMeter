using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;

namespace WebPerformanceMeter.PerformancePlans
{
    public interface IUsersPerformancePlan
    {
        IBaseUser User { get; }

        Task StartAsync();
    }
}
