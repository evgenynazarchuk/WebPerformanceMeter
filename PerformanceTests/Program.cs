namespace PerformanceTests
{
    using System.Reflection;
    using System.Threading.Tasks;
    using WebPerformanceMeter.Support;

    public class Program
    {
        public static async Task Main()
        {
            await WebPerformanceRunner.ManualAsync(Assembly.GetExecutingAssembly());
        }
    }
}
