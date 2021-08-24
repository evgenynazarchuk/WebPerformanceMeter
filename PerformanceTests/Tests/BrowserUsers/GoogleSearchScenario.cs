namespace PerformanceTests.Tests.BrowserUsers
{
    using System.Threading.Tasks;
    using WebPerformanceMeter.Attributes;
    using WebPerformanceMeter.Extensions;
    using WebPerformanceMeter.PerformancePlans;
    using WebPerformanceMeter.Support;

    public class GoogleSearchScenario
    {
        [PerformanceTest(1, 5)]
        [PerformanceTest(5, 5)]
        public async Task GoGoogle(int minutes, int usersCount)
        {
            // Arrange
            var user = new GoogleSearchUser();
            var plan = new ActiveUsersOnPeriod(user, usersCount, minutes.Minutes(), 3);

            // Act
            await new Scenario()
                .AddSequentialPlans(plan)
                .StartAsync();
        }
    }
}
