namespace WebPerformanceMeter.Interfaces
{
    public interface IDataReader<TData>
        where TData : class
    {
        TData? GetData();

        //void ProcessFile(string path, bool hasHeader = false, string separator = ",", bool cyclicalData = false);
    }
}
