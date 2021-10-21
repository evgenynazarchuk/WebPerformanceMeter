using System;
using System.Threading.Tasks;

namespace WebPerformanceMeter.Interfaces
{
    public interface IUser : IBaseUser
    {
        Task InvokeAsync(int userLoopCount);

        Task PerformanceAsync();
    }
}
