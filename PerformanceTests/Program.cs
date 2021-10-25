using System.Threading.Tasks;
using System.Reflection;
using WebPerformanceMeter.Support;
using WebPerformanceMeter;

namespace PerformanceTests
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var runner = new Runner(args, Assembly.GetExecutingAssembly());
            await runner.StartAsync();
        }
    }
}
