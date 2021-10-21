using System.Threading.Tasks;
using WebPerformanceMeter.Attributes;
using WebPerformanceMeter.Extensions;
using WebPerformanceMeter.PerformancePlans;
using WebPerformanceMeter.Support;
using WebPerformanceMeter.Tools.WebSocketTool;
using WebPerformanceMeter.Users.WebSocket;

namespace WebSocketWebApplication.PerformanceTest.Tests
{
    public class SendMessageTest
    {
        [PerformanceTest(5, 50)]
        public async Task SendMessageHelloWorldTest(int seconds, int usersCount)
        {
            var user = new WebSocketUserTest("localhost", 5000, "ws");
            var plan = new ActiveUsersOnPeriod(user, usersCount, seconds.Seconds());

            await new Scenario()
                .AddSequentialPlans(plan)
                .StartAsync();
        }

        public class WebSocketUserTest : WebSocketUser
        {
            public WebSocketUserTest(string host, int port, string path) : base(host, port, path) { }

            protected override async Task PerformanceAsync(WebSocketTool client)
            {
                await SendMessageAsync(client, "Hello world");
            }
        }
    }
}
