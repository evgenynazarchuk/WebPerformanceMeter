using System.Threading.Tasks;
using Tests.Tests.ActiveUserOnPeriodBase;

namespace Tests
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ActiveUserOnPeriodBaseTest test = new();
            await test.RunAsync();
        }
    }
}
