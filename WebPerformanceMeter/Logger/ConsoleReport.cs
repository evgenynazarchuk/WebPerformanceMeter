using System;
using System.Threading.Tasks;

namespace WebPerformanceMeter.Logger
{
    public class ConsoleReport : AsyncReport
    {
        private long _counter = 0;

        public ConsoleReport()
        {
            Console.Clear();
        }

        public override async Task WriteAsync(string message)
        {
            Console.Write($"Completed Requests: {++this._counter}\r");

            await Task.CompletedTask;
        }
    }
}
