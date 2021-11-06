using System;
using System.Reflection;
using System.Threading.Tasks;
using WebPerformanceMeter.Support;
using WebPerformanceMeter;

namespace RestWebApplication.WebPerformanceTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var runner = new TestRunner(args, Assembly.GetExecutingAssembly());
            await runner.StartAsync();
        }
    }
}
