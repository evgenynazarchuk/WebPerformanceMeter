using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebPerformanceMeter.Attributes;
using WebPerformanceMeter.Extensions;
using WebPerformanceMeter.Support;
using WebPerformanceMeter;


namespace PerformanceTests.Tests.HttpClientTests
{
    [PerformanceClass]
    public class Demo10
    {
        public const string ADDRESS = "https://localhost:5001";

        [PerformanceTest(5000, 30 * 60)]
        public async Task CreateEntityTest(int usersCountPerSecond, int seconds)
        {
            var user = new CreateEntityUser(ADDRESS);
            var plan = new UsersPerPeriod(user, usersCountPerSecond, seconds.Seconds());
            await new Scenario()
                .AddSequentialPlans(plan)
                .Start();
        }
    }

    public class CreateEntityUser : HttpUser
    {
        public CreateEntityUser(string address)
            : base(address) { }
        protected override async Task Performance()
        {
            await PostAsJson<Message, Message>("/home/CreateEntity", new Message { Text = "Hello world" });
        }
    }

    public class Message
    {
        public int Id { get; set; }

        public string Text {  get; set; }
    }
}
