using PerformanceTests.Tests.Users;
using WebPerformanceMeter.Extensions;
using WebPerformanceMeter.PerformancePlans;
using WebPerformanceMeter.Attributes;
using WebPerformanceMeter.Support;
using System.Threading.Tasks;

namespace PerformanceTests.Tests.Scenarios
{
    public class SampleTest2
    {
        [PerformanceTest]
        public async Task FewActs()
        {
            var app = new WebApplication();
            var user1 = new TestWaitUser1(app.HttpClient);
            var user2 = new TestWaitUser2(app.HttpClient);
            var plan1 = new ActiveUsersOnPeriod(user1, 200, 10.Minutes());
            var plan2 = new ActiveUsersOnPeriod(user2, 200, 10.Minutes());
            var plan3 = new ActiveUsersOnPeriod(user1, 200, 10.Minutes());

            await new Scenario()
                .AddSequentialPlans(plan1)
                .AddParallelPlans(plan2, plan3)
                .StartAsync();
        }
    }
}
