namespace WebPerformanceMeter.Logger
{
    using System;
    using System.Threading.Tasks;

    public class ConsoleReport : IAsyncReport
    {
        private long counter = 0;

        public ConsoleReport()
        {
            Console.Clear();
        }

        public async Task WriteAsync(string fileName, string message)
        {
            Console.Write($"Completed Requests: {++this.counter}\r");

            await Task.CompletedTask;
        }
    }
}
