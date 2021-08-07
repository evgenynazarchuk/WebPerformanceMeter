using System.Threading.Tasks;
using PerformanceTests.Tests.Scenarios;

namespace PerformanceTests
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await SampleTest2.Test();
        }
    }
}
