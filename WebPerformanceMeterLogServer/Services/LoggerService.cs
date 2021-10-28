using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System.Threading.Tasks;

namespace WebPerformanceMeterLogServer
{
    public class LoggerService : Logger.LoggerBase
    {
        public LoggerService()
        {
        }

        public override async Task<Empty> SendLogMessage(
            IAsyncStreamReader<LogMessageCreateDto> requestStream,
            ServerCallContext context)
        {
            // read stream
            while (await requestStream.MoveNext())
            {
                // requestStream.Current.TestRunId
            }

            // save
            //await this.dataContext.SaveChangesAsync();

            return new Empty();
        }
    }
}
