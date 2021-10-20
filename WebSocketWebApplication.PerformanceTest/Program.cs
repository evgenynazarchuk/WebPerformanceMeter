using System.Reflection;
using System.Threading.Tasks;
using WebPerformanceMeter.Support;

namespace WebSocketWebApplication.PerformanceTest
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await WebPerformanceRunner.ManualAsync(Assembly.GetExecutingAssembly());
        }
    }
}
