using System.Reflection;
using WebPerformanceMeter.Support;

namespace PerformanceTests
{
    class Program
    {
        static void Main()
        {
            WebPerformanceRunner.Manual(Assembly.GetExecutingAssembly());
        }
    }
}
