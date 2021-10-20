using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;

namespace WebPerformanceMeter.Users
{
    public interface IUser
    {
        public string UserName { get; }

        Task InvokeAsync(int userLoopCount, IDataReader? dataSource, bool reuseDataValueInLoop);
    }
}
