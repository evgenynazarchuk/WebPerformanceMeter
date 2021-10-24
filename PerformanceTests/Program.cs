using System.Reflection;
using WebPerformanceMeter.Support;

namespace PerformanceTests
{
    public class Program
    {
        //public static async Task Main()
        //{
        //    await WebPerformanceRunner.ManualAsync(Assembly.GetExecutingAssembly());
        //}

        public static void Main()
        {
            WebPerformanceRunner.WebService(Assembly.GetExecutingAssembly());
        }
    }
}
