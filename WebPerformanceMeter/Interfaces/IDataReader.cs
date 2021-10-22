namespace WebPerformanceMeter.Interfaces
{
    public interface IDataReader<TData>
        where TData : class
    {
        TData? GetData();
    }
}
