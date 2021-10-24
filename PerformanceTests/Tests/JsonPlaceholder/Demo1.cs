using System.Threading.Tasks;
using WebPerformanceMeter;
using WebPerformanceMeter.Attributes;
using WebPerformanceMeter.Extensions;
using WebPerformanceMeter.Support;

namespace PerformanceTests.Tests.JsonPlaceholder
{
    [PerformanceClass]
    public class Demo1
    {
        private readonly string _address = "https://jsonplaceholder.typicode.com";

        [PerformanceTest(10, 10)]
        [PerformanceTest(20, 10)]
        [PerformanceTest(30, 10)]
        public async Task GetAllPostsWithoutResultTest(int activeUsersCount, int seconds)
        {
            var user = new UserAction(this._address);
            var plan = new ActiveUsersOnPeriod(user, activeUsersCount, seconds.Seconds());

            await new Scenario()
                .AddSequentialPlans(plan)
                .Start();
        }

        public class UserAction : HttpUser
        {
            public UserAction(string address)
                : base(address) { }

            protected override async Task Performance()
            {
                await Get("/posts");
            }
        }
    }
}
