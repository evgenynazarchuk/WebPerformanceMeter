namespace WebPerformanceMeter.Interfaces
{
    public interface ITypedWebSocketUser<TEntity> : IBaseWebSocketUser, ITypedUser<TEntity>
        where TEntity : class
    {
    }
}
