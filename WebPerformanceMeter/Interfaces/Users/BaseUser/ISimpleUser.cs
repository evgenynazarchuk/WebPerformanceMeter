using System.Threading.Tasks;

namespace WebPerformanceMeter.Interfaces
{
    public interface ISimpleUser : IBaseUser
    {
        Task InvokeAsync(int userLoopCount);

        //Task PerformanceAsync();
    }
}
