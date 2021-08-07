using PerformanceTests.Tests.Users;
using WebPerformanceMeter.Extensions;
using WebPerformanceMeter.PerformancePlans;
using WebPerformanceMeter.Attributes;
using WebPerformanceMeter.Support;

namespace PerformanceTests.Tests.Scenarios
{
    [TestClass]
    public class SampleTest1
    {
        [Test]
        public void TwoParallelUsers()
        {
            // Arange
            var app = new WebApplication();
            var user1 = new TestWaitUser1(app.HttpClient);
            var user2 = new TestWaitUser2(app.HttpClient);
            var plan1 = new ActiveUsersOnPeriod(user1, 200, 30.Minutes());
            var plan2 = new ActiveUsersOnPeriod(user2, 200, 30.Minutes());

            // Run test
            new Scenario()
                .AddParallelPlans(plan1, plan2)
                .RunAsync().Wait();
        }
    }
}
