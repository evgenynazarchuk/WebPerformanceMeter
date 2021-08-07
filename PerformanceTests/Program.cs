using System.Reflection;
using WebPerformanceMeter.Runner;

namespace PerformanceTests
{
    class Program
    {
        static void Main()
        {
            Runner.Manual(Assembly.GetExecutingAssembly());
        }
    }
}
