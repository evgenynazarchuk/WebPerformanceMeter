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
        public static async Task Main()
        {
            await WebPerformanceRunner.ManualAsync(Assembly.GetExecutingAssembly());
        }
    }
}
