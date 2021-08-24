namespace WebPerformanceMeter.Logger
{
    using System.Threading.Tasks;

    public class GrpcReport : AsyncReport
    {
        public override async Task WriteAsync(string message)
        {
            await Task.CompletedTask;
        }
    }
}
