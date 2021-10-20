using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Logger;

namespace WebPerformanceMeter.Users
{
    public interface IUser
    {
        string UserName { get; }

        ILogger Logger { get; }

        Task InvokeAsync(int userLoopCount, IDataReader? dataReader, bool reuseDataValueInLoop);
    }
}
