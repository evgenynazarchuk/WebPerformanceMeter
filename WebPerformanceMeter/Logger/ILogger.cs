namespace WebPerformanceMeter.Logger
{
    using System.Threading.Tasks;

    public interface ILogger
    {
        void Write(string message);

        void Finish();

        Task StartProcessingAsync();

        void StopProcessing();
    }
}
