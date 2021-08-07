using System.Threading.Tasks;
using PerformanceTests.Tests.Users;
using WebPerformanceMeter.Extensions;
using WebPerformanceMeter.PerformancePlans;
using WebPerformanceMeter.Support;

namespace PerformanceTests.Tests.Scenarios
{
    public static class SampleTest2
    {
        public static async Task Test()
        {
            var app = new WebApplication();
            var user1 = new TestWaitUser1(app.HttpClient);
            var user2 = new TestWaitUser2(app.HttpClient);
            var user3 = new TestWaitUser3(app.HttpClient);
            var plan1 = new ActiveUsersOnPeriod(user1, 200, 10.Seconds());
            var plan2 = new ActiveUsersOnPeriod(user2, 200, 10.Seconds());
            var plan3 = new ActiveUsersOnPeriod(user3, 200, 10.Seconds());

            var scenario = new Scenario();
            scenario.AddSequentialPlans(plan3);
            scenario.AddParallelPlans(plan1, plan2);
            
            await scenario.RunAsync();
        }
    }
}
