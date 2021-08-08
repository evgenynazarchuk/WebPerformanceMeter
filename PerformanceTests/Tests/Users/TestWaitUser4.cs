using System.Net.Http;
using System.Threading.Tasks;
using TestWebApiServer.Models;

namespace PerformanceTests.Tests.Users
{
    public class TestWaitUser4 : TestUserFacade
    {
        // Arange
        public readonly TestRequestContent content = new() { Timeout = 400 };

        public TestWaitUser4(HttpClient client)
            : base(client) { }

        protected override async Task PerformanceAsync()
        {
            // Action
            await TestWaitMethod2(content, "400ms");
        }
    }
}
