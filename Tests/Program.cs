using System;
using Tests.Tests.ConstantUsersTests;
using System.Threading.Tasks;

namespace Tests
{
    class Program
    {
        static async Task Main(string[] args)
        {
            TestPerformance test = new();
            await test.RunAsync();
        }
    }
}
