using WebPerformanceMeter.Interfaces;

namespace WebPerformanceMeter.Users
{
    public abstract class BaseUser : IBaseUser
    {
        public string UserName { get => this.userName; }
        
        public ILogger? Logger { get => this.logger; }

        protected readonly string userName;

        protected readonly ILogger? logger;

        public BaseUser(string userName, ILogger? logger = null)
        {
            this.userName = userName;
            this.logger = logger;
        }
    }
}