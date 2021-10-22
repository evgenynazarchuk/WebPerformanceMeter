using System.Threading.Tasks;
using WebPerformanceMeter;
using WebPerformanceMeter.Attributes;
using WebPerformanceMeter.Support;

namespace PerformanceTests.Tests.JsonPlaceholder
{
    public class Demo1
    {
        private readonly string _address = "https://jsonplaceholder.typicode.com";

        [PerformanceTest(5)]
        public async Task GetAllPostsWithoutResultTest(int usersCount)
        {
            var user = new UserAction(this._address);
            var plan = new ConstantUsers(user, usersCount);

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
