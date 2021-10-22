using System.Threading.Tasks;

namespace WebPerformanceMeter.Interfaces
{
    public interface ITypedUser<TEntity> : IBaseUser
        where TEntity : class
    {
        Task InvokeAsync(IDataReader<TEntity> dataReader, bool reuseDataInLoop = false, int userLoopCount = 1);

        //Task PerformanceAsync(TEntity data);
    }
}
