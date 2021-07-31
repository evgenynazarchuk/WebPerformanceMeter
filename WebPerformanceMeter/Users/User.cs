using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;

namespace WebPerformanceMeter.Users
{
    public abstract class User
    {
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
