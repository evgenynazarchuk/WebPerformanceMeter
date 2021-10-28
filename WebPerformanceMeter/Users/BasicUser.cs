using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Reports;

namespace WebPerformanceMeter.Users
{
    public abstract class BasicUser : IBasicUser
    {
        public readonly Watcher Watcher;

        public readonly string UserName;

        public BasicUser(string userName)
        {
            this.Watcher = new Watcher();
            this.UserName = userName;
        }
    }
}