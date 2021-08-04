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

        public override async Task WriteErrorAsync(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();

            await Task.CompletedTask;
        }
    }
}
