using System.Threading.Tasks;
using PerformanceTests.Tests.Scenarios;

namespace PerformanceTests
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await ReuseTestWaitUser.Test();
        }
    }
}
