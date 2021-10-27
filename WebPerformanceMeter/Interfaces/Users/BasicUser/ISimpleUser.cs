using System.Threading.Tasks;

namespace WebPerformanceMeter.Interfaces
{
    public interface ISimpleUser : IBasicUser
    {
        Task InvokeAsync(int userLoopCount);

        //Task PerformanceAsync();
    }
}
