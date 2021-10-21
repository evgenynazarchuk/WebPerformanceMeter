using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;

namespace WebPerformanceMeter
{
    public abstract class BaseUser : IBaseUser
    {
        public string UserName { get; private set; }

        public ILogger? Logger { get; private set; }

        public BaseUser()
        {
            this.UserName = string.Empty;
        }

        public virtual void SetUserName(string userName)
        {
            this.UserName = userName;
        }

        public virtual void SetLogger(ILogger logger)
        {
            this.Logger = logger;
        }
    }
}