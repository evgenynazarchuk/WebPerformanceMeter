using System.Net.Http;
using System.Threading.Tasks;
using TestWebApiServer.Models;

namespace PerformanceTests.Tests.Users
{
    public class TestWaitUser2 : TestUserFacade
    {
        // Arange
        public readonly TestRequestContent content = new TestRequestContent { Timeout = 200 };

        public TestWaitUser2(HttpClient client)
            : base(client) { }

        public override async Task PerformanceAsync()
        {
            // Act
            await TestWaitMethod2(content, "200ms");
        }
    }
}
