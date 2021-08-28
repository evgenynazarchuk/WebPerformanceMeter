/// <summary>
/// 
/// </summary>
namespace PerformanceTests
{
    using System.Reflection;
    using System.Threading.Tasks;
    using WebPerformanceMeter.Support;

    public class Program
    {
        /// <summary>
        /// doc
        /// </summary>
        /// <returns>Task</returns>
        /// version 1
        public static async Task Main()
        {
            await WebPerformanceRunner.ManualAsync(Assembly.GetExecutingAssembly());
        }
    }
}
