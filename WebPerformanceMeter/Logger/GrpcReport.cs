using System.Threading.Tasks;

namespace WebPerformanceMeter.Logger
{
    public class GrpcReport : AsyncReport
    {
        public override async Task WriteAsync(string message)
        {
            await Task.CompletedTask;
        }

        public override async Task WriteErrorAsync(string message)
        {
            await Task.CompletedTask;
        }
    }
}
