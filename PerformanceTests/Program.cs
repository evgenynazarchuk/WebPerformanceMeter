using System.Reflection;
using WebPerformanceMeter.Support;
using System.Threading.Tasks;

namespace PerformanceTests
{
    class Program
    {
        static async Task Main()
        {
            await WebPerformanceRunner.ManualAsync(Assembly.GetExecutingAssembly());
        }
    }
}
