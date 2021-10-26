using System.Reflection;
using System.Threading.Tasks;
using WebPerformanceMeter;

namespace WebSocketWebApplication.PerformanceTest
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var runner = new TestRunner(args, Assembly.GetExecutingAssembly());
            await runner.StartAsync();
        }
    }
}
