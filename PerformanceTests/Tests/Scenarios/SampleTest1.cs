namespace PerformanceTests.Tests.Scenarios
{
    using System.Threading.Tasks;
    using PerformanceTests.Tests.Users;
    using WebPerformanceMeter.Attributes;
    using WebPerformanceMeter.Extensions;
    using WebPerformanceMeter.PerformancePlans;
    using WebPerformanceMeter.Support;

    public class GoogleSearchScenatio
    {
        [PerformanceTest(1, 200)]
        [PerformanceTest(10, 200)]
        [PerformanceTest(30, 200)]
        [PerformanceTest(60, 200)]
        [PerformanceTest(90, 200)]
        public async Task TwoParallelUsers(int minutes, int usersCount)
        {
            // Arrange
            var app = new WebApplication();

            var user1 = new TestWaitUser1(app.HttpClient);
            var user2 = new TestWaitUser2(app.HttpClient);

            var plan1 = new ActiveUsersOnPeriod(user1, usersCount, minutes.Minutes());
            var plan2 = new ActiveUsersOnPeriod(user2, usersCount, minutes.Minutes());

            // Run test
            await new Scenario()
                .AddParallelPlans(plan1, plan2)
                .StartAsync();
        }
    }
}
