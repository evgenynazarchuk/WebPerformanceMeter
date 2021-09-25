namespace WebPerformanceMeter.Interfaces
{
    public interface IEntityReader
    {
        object? GetEntity();

        void ProcessCsvFile(string path, bool hasHeader = false, bool cyclicalData = false, string separator = ",");
    }
}
