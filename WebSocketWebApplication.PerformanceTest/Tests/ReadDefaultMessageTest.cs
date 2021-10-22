using System.Threading.Tasks;
using WebPerformanceMeter;
using WebPerformanceMeter.Attributes;
using WebPerformanceMeter.Extensions;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Support;

namespace WebSocketWebApplication.PerformanceTest.Tests
{
    public class ReadDefaultMessageTest
    {
        [PerformanceTest(5, 50)]
        public async Task WebSocketBaseTest(int seconds, int usersCount)
        {
            var user = new WebSocketUserTest("localhost", 5000, "ws");
            var plan = new ActiveUsersOnPeriod(user, usersCount, seconds.Seconds());

            await new Scenario()
                .AddSequentialPlans(plan)
                .Start();
        }

        public class WebSocketUserTest : WebSocketUser
        {
            public WebSocketUserTest(string host, int port, string path) : base(host, port, path) { }

            protected override async Task PerformanceAsync(IWebSocketTool client)
            {
                await ReceiveMessage(client);
            }
        }
    }
}
