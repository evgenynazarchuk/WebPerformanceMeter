using System.Reflection;
using System.Threading.Tasks;
using WebPerformanceMeter.Support;

namespace PerformanceTests
{
    public class Program
    {
        public static async Task Main()
        {
            await WebPerformanceRunner.ManualAsync(Assembly.GetExecutingAssembly());
        }
    }
}
