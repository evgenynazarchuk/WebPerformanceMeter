namespace WebPerformanceMeter.Users
{
    using System.Threading.Tasks;
    using WebPerformanceMeter.Interfaces;
    using WebPerformanceMeter.Logger;

    public abstract class User
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

        public abstract Task InvokeAsync(int loopCount, IEntityReader? dataReader, bool reuseDataInLoop);
    }
}