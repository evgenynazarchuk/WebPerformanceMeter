using System;
using System.Threading.Tasks;

namespace WebPerformanceMeter.Logger
{
    public class ConsoleReport : AsyncReport
    {
        public override async Task WriteAsync(string message)
        {
            Console.WriteLine(message);

            await Task.CompletedTask;
        }
    }
}
