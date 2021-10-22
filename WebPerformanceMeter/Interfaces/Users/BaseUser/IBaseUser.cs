namespace WebPerformanceMeter.Interfaces
{
    public interface IBaseUser
    {
        string UserName { get; }

        ILogger? Logger { get; }
    }
}
