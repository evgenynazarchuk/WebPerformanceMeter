using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPerformanceMeter.Interfaces
{
    public interface ITypedUser<TEntity> : IBaseUser
        where TEntity : class
    {
        Task InvokeAsync(int userLoopCount, IDataReader? dataReader = null, bool reuseDataInLoop = false);

        Task PerformanceAsync(TEntity data);
    }
}
