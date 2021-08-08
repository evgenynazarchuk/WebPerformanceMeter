using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;

namespace WebPerformanceMeter.Users
{
    public abstract class User
    {
        private string UserName;

        public User()
        {
            UserName = this.GetType().Name;
        }

        protected void SetUserName(string userName = "")
        {
            if (string.IsNullOrEmpty(userName))
            {
                UserName = this.GetType().Name;
            }
            else
            {
                UserName = userName;
            }
        }

        public async Task InvokeAsync(
            int loopCount = 1,
            IEntityReader? dataReader = null,
            bool reuseDataInLoop = true
            )
        {
            object? entity = null;

            if (dataReader is not null)
            {
                entity = dataReader.GetEntity();

                if (entity is null)
                {
                    return;
                }
            }

            for (int i = 0; i < loopCount; i++)
            {
                if (entity is null)
                {
                    await PerformanceAsync();
                }
                else
                {
                    await PerformanceAsync(entity);
                }

                if (dataReader is not null && !reuseDataInLoop)
                {
                    entity = dataReader.GetEntity();

                    if (entity is null)
                    {
                        return;
                    }
                }
            }
        }

        protected virtual Task PerformanceAsync(object entity)
        {
            return Task.CompletedTask;
        }

        protected virtual Task PerformanceAsync()
        {
            return Task.CompletedTask;
        }
    }
}
