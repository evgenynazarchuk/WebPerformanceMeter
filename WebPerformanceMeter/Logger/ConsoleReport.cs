namespace WebPerformanceMeter.Logger
{
    using System;
    using System.Threading.Tasks;

    public class ConsoleReport : AsyncReport
    {
        private long counter = 0;

        public ConsoleReport()
        {
            Console.Clear();
        }

        public override async Task WriteAsync(string message)
        {
            Console.Write($"Completed Requests: {++this.counter}\r");

            await Task.CompletedTask;
        }
    }
}
