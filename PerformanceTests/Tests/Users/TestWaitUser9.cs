using System.Net.Http;
using System.Threading.Tasks;
using TestWebApiServer.Models;

namespace PerformanceTests.Tests.Users
{
    public class TestWaitUser9 : TestUserFacade
    {
        // Arange
        public readonly TestRequestContent content = new() { Timeout = 900 };

        public TestWaitUser9(HttpClient client)
            : base(client) { }

        protected override async Task PerformanceAsync()
        {
            // Action
            await TestWaitMethod1(content, "900ms");
        }
    }
}
