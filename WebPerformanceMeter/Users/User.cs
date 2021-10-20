using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Logger;

namespace WebPerformanceMeter.Users
{
    public abstract class User : IUser
    {
        public string UserName { get; private set; }

        public readonly ILogger Logger;

        public User(ILogger logger)
        {
            this.UserName = string.Empty;
            this.Logger = logger;
        }

        protected void SetUserName(string userName)
        {
            this.UserName = userName;
        }

        public abstract Task InvokeAsync(int userLoopCount, IDataReader? dataSource, bool reuseDataInLoop);
    }
}