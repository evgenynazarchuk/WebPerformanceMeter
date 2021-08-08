using System.Net.Http;
using System.Threading.Tasks;
using TestWebApiServer.Models;

namespace PerformanceTests.Tests.Users
{
    public class TestWaitUser6 : TestUserFacade
    {
        // Arange
        public readonly TestRequestContent content = new() { Timeout = 600 };

        public TestWaitUser6(HttpClient client)
            : base(client) { }

        public override async Task PerformanceAsync()
        {
            // Action
            await TestWaitMethod2(content, "600ms");
        }
    }
}
