namespace WebPerformanceMeter.Logger
{
    using System.Threading.Tasks;

    public interface IPerformanceLogger
    {
        void AppendToolLogMessage(string message);

        void AppendUserLogMessage(string message);

        void UserWriteLogSerialize(string message);

        void ToolWriteLogSerialize(string message);

        Task StartProcessingAsync();

        void StopProcessing();

        void Finish();

        void PostProcessing();
    }
}
