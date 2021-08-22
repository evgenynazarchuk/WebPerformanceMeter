using PerformanceTests.Tests.Users;
using WebPerformanceMeter.Extensions;
using WebPerformanceMeter.PerformancePlans;
using WebPerformanceMeter.Attributes;
using WebPerformanceMeter.Support;
using System.Threading.Tasks;
using System;

namespace PerformanceTests.Tests.BrowserUsers
{
    public class GoogleSearchScenario
    {
        [PerformanceTest(1, 5)]
        [PerformanceTest(5, 5)]
        public async Task GoGoogle(int minutes, int usersCount)
        {
            // Arrange
            var user = new GoogleSearchUser();
            var plan = new ActiveUsersOnPeriod(user, usersCount, minutes.Minutes());

            // Act
            await new Scenario()
                .AddSequentialPlans(plan)
                .StartAsync();
        }
    }
}
