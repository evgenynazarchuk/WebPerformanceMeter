using System.Threading.Tasks;
using WebPerformanceMeter;
using WebPerformanceMeter.Attributes;
using WebPerformanceMeter.Extensions;
using WebPerformanceMeter.Support;

namespace PerformanceTests.Tests.JsonPlaceholder
{
    [PerformanceClass]
    public class Demo11
    {
        private readonly string _address = "https://jsonplaceholder.typicode.com";

        [PerformanceTest()]
        public async Task GetAllPostsWithoutResultTest()
        {
            var user = new UserAction(this._address);
            var plan = new ActiveUsersOnPeriod(user, 10, 30.Seconds());

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
