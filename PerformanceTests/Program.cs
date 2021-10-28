using System.Reflection;
using System.Threading.Tasks;
using WebPerformanceMeter;

namespace PerformanceTests
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var runner = new TestRunner(args, Assembly.GetExecutingAssembly());
            await runner.StartAsync();
        }
    }
}
