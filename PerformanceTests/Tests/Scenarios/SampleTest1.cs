using PerformanceTests.Tests.Users;
using WebPerformanceMeter.Extensions;
using WebPerformanceMeter.PerformancePlans;
using WebPerformanceMeter.Attributes;
using WebPerformanceMeter.Support;
using System.Threading.Tasks;

namespace PerformanceTests.Tests.Scenarios
{
    public class SampleTest1
    {
        [PerformanceTest]
        public async Task TwoParallelUsers()
        {
            // Arange
            var app = new WebApplication();
            var user1 = new TestWaitUser1(app.HttpClient);
            var user2 = new TestWaitUser2(app.HttpClient);
            var plan1 = new ActiveUsersOnPeriod(user1, 200, 90.Minutes());
            var plan2 = new ActiveUsersOnPeriod(user2, 200, 90.Minutes());

            // Run test
            await new Scenario()
                .AddParallelPlans(plan1, plan2)
                .StartAsync();
        }
    }
}
