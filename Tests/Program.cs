using System;
using Tests.Tests;
using System.Threading.Tasks;
using Tests.Tests.UserOnPeriodBase;

namespace Tests
{
    class Program
    {
        static async Task Main(string[] args)
        {
            UserOnPeriodBaseTest test = new();
            await test.RunAsync();
        }
    }
}
