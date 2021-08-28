namespace WebPerformanceMeter.Users
{
    using System.Threading.Tasks;
    using WebPerformanceMeter.Interfaces;

    public abstract class User
    {
        public User()
        {
            this.UserName = string.Empty;
        }

        protected void SetUserName(string userName)
        {
            this.UserName = userName;
        }

        public string UserName { get; private set; }

        public abstract Task InvokeAsync(int loopCount, IEntityReader? dataReader, bool reuseDataInLoop);
    }
}