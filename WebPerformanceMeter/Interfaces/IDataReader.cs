namespace WebPerformanceMeter.Interfaces
{
    public interface IDataReader
    {
        object? GetEntity();

        void ProcessFile(string path, bool hasHeader = false, bool cyclicalData = false, string separator = ",");
    }
}
