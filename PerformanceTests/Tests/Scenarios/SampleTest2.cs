using PerformanceTests.Tests.Users;
using WebPerformanceMeter.Extensions;
using WebPerformanceMeter.PerformancePlans;
using WebPerformanceMeter.Attributes;
using WebPerformanceMeter.Support;

namespace PerformanceTests.Tests.Scenarios
{
    [TestClass]
    public class SampleTest2
    {
        [Test]
        public void FewActs()
        {
            var app = new WebApplication();
            var user1 = new TestWaitUser1(app.HttpClient);
            var user2 = new TestWaitUser2(app.HttpClient);
            var user3 = new TestWaitUser3(app.HttpClient);
            var plan1 = new ActiveUsersOnPeriod(user1, 200, 30.Minutes());
            var plan2 = new ActiveUsersOnPeriod(user2, 200, 30.Minutes());
            var plan3 = new ActiveUsersOnPeriod(user3, 200, 30.Minutes());

            new Scenario()
                .AddSequentialPlans(plan3)
                .AddParallelPlans(plan1, plan2)
                .RunAsync().Wait();
        }
    }
}
