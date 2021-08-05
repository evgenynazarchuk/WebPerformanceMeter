using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;

namespace WebPerformanceMeter.Users
{
    public abstract class User
    {
        protected string UserName;

        public User()
        {
            UserName = this.GetType().Name;
        }

        public void SetUserName(string userName)
        {
            UserName = userName;
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

        public virtual Task PerformanceAsync(object entity)
        {
            return Task.CompletedTask;
        }

        public virtual Task PerformanceAsync()
        {
            return Task.CompletedTask;
        }
    }
}
