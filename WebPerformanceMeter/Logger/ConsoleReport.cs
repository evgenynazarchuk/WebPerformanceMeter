using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            Console.WriteLine(message);

            await Task.CompletedTask;
        }
    }
}
