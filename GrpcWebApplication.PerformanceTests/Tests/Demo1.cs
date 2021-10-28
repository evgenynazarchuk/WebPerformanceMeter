using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebPerformanceMeter;
using WebPerformanceMeter.Attributes;
using WebPerformanceMeter.Extensions;
using WebPerformanceMeter.Support;
using GrpcWebApplication.PerformanceTests.Users;

namespace GrpcWebApplication.PerformanceTests.Tests
{
    [PerformanceClass]
    public class Demo1
    {
        private const string ADDRESS = "https://localhost:5001";

        [PerformanceTest(10, 120)]
        public async Task UnaryCallByActiveUsersTest(int activeUsers, int seconds)
        {
            var user = new UnaryGrpcUser(ADDRESS);
            var plan = new ActiveUsersOnPeriod(user, activeUsers, seconds.Seconds());

            await new Scenario()
                .AddSequentialPlans(plan)
                .Start();
        }

        [PerformanceTest(500, 20 * 60)]
        public async Task UnaryCallByUsersPerPeriodTest(int users, int seconds)
        {
            var user = new UnaryGrpcUser(ADDRESS);
            var plan = new UsersPerPeriod(user, users, seconds.Seconds());

            await new Scenario()
                .AddSequentialPlans(plan)
                .Start();
        }

        [PerformanceTest(5, 60)]
        public async Task ClientStreamTest(int users, int seconds)
        {
            var user = new ClientStreamUser(ADDRESS);
            var plan = new ActiveUsersOnPeriod(user, users, seconds.Seconds());

            await new Scenario()
                .AddSequentialPlans(plan)
                .Start();
        }
    }
}
