namespace WebPerformanceMeter.Interfaces
{
    public interface IDataReader<TEntity>
        where TEntity : class
    {
        TEntity? GetData();

        void ProcessFile(string path, bool hasHeader = false, string separator = ",", bool cyclicalData = false);
    }
}
