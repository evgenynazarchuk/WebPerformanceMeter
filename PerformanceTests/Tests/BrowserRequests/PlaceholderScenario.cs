namespace PerformanceTests.Tests.BrowserRequests
{
    using System.Threading.Tasks;
    using WebPerformanceMeter.Attributes;
    using WebPerformanceMeter.Extensions;
    using WebPerformanceMeter.PerformancePlans;
    using WebPerformanceMeter.Support;

    public class PlaceholderScenario
    {
        [PerformanceTest(1, 5)]
        [PerformanceTest(5, 5)]
        public async Task PostToPlaceHolder(int minutes, int usersCount)
        {
            // Arrange
            var user = new PlaceholderUserRequest();
            var plan = new ActiveUsersOnPeriod(user, usersCount, minutes.Minutes());

            // Act
            await new Scenario()
                .AddSequentialPlans(plan)
                .StartAsync();
        }
    }
}
