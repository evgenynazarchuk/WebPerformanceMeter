namespace PerformanceTests.Tests.Scenarios
{
    using PerformanceTests.Tests.Users;
    using System.Threading.Tasks;
    using WebPerformanceMeter.Attributes;
    using WebPerformanceMeter.Extensions;
    using WebPerformanceMeter.PerformancePlans;
    using WebPerformanceMeter.Support;

    public class SampleTest3
    {
        [PerformanceTest(1, 200)]
        [PerformanceTest(10, 200)]
        [PerformanceTest(30, 200)]
        [PerformanceTest(60, 200)]
        [PerformanceTest(90, 200)]
        public async Task FiveUsers(int minutes, int usersCount)
        {
            var app = new WebApplication();

            var user1 = new TestWaitUser1(app.HttpClient);
            var user2 = new TestWaitUser2(app.HttpClient);
            var user3 = new TestWaitUser3(app.HttpClient);
            var user4 = new TestWaitUser4(app.HttpClient);
            var user5 = new TestWaitUser5(app.HttpClient);
            var user6 = new TestWaitUser6(app.HttpClient);
            var user7 = new TestWaitUser7(app.HttpClient);
            var user8 = new TestWaitUser8(app.HttpClient);
            var user9 = new TestWaitUser9(app.HttpClient);
            var user10 = new TestWaitUser10(app.HttpClient);
            var user11 = new TestWaitUser11(app.HttpClient);

            var plan1 = new ActiveUsersOnPeriod(user1, usersCount, minutes.Minutes());
            var plan2 = new ActiveUsersOnPeriod(user2, usersCount, minutes.Minutes());
            var plan3 = new ActiveUsersOnPeriod(user3, usersCount, minutes.Minutes());
            var plan4 = new ActiveUsersOnPeriod(user4, usersCount, minutes.Minutes());
            var plan5 = new ActiveUsersOnPeriod(user5, usersCount, minutes.Minutes());
            var plan6 = new ActiveUsersOnPeriod(user6, usersCount, minutes.Minutes());
            var plan7 = new ActiveUsersOnPeriod(user7, usersCount, minutes.Minutes());
            var plan8 = new ActiveUsersOnPeriod(user8, usersCount, minutes.Minutes());
            var plan9 = new ActiveUsersOnPeriod(user9, usersCount, minutes.Minutes());
            var plan10 = new ActiveUsersOnPeriod(user10, usersCount, minutes.Minutes());
            var plan11 = new ActiveUsersOnPeriod(user11, usersCount, minutes.Minutes());

            await new Scenario()
                .AddParallelPlans(plan1, plan2, plan3, plan4, plan5, plan6, plan7, plan8, plan9, plan10, plan11)
                .StartAsync();
        }
    }
}
