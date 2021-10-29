using System.Threading.Tasks;
using WebPerformanceMeter;
using WebPerformanceMeter.Attributes;
using WebPerformanceMeter.Extensions;
using WebPerformanceMeter.Support;

namespace WebSocketWebApplication.PerformanceTest.Tests
{
    [PerformanceClass]
    public class DelaySendMessageTests
    {
        [PerformanceTest(120, 500)]
        public async Task SendMessageHelloWorldTest(int seconds, int usersCountPerPeriod)
        {
            var user = new WebSocketUserTest("localhost", 5000, "ws");
            var plan = new UsersPerPeriod(user, usersCountPerPeriod, seconds.Seconds());

            await new Scenario()
                .AddSequentialPlans(plan)
                .Start();
        }

        [PerformanceTest(120, 500)]
        public async Task ActiveUsersSendMessageHelloWorldTest(int seconds, int activeUsersCount)
        {
            var user = new WebSocketUserTest("localhost", 5000, "ws");
            var plan = new ActiveUsersOnPeriod(user, activeUsersCount, seconds.Seconds());

            await new Scenario()
                .AddSequentialPlans(plan)
                .Start();
        }

        public class WebSocketUserTest : WebSocketUser
        {
            public WebSocketUserTest(string host, int port, string path) : base(host, port, path) { }

            protected override async Task PerformanceAsync(WebSocketTool client)
            {
                await Task.Delay(500);
                await SendMessage(client, "Hello world");
            }
        }
    }
}
