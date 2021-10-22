using System.Net.Http;
using System.Threading.Tasks;
using TestWebApiServer.Models;
using System.Collections.Generic;
using WebPerformanceMeter.Attributes;
using WebPerformanceMeter.Extensions;
using WebPerformanceMeter.PerformancePlans;
using WebPerformanceMeter.Support;
using WebPerformanceMeter.Users;
using WebPerformanceMeter;
using PerformanceTests.Tests.JsonPlaceholder.Dto;

namespace PerformanceTests.Tests.JsonPlaceholder
{
    public class Demo1
    {
        [PerformanceTest(5)]
        public async Task GetAllPostsWithoutResultTest(int usersCount)
        {
            var address = "https://jsonplaceholder.typicode.com";
            var user = new UserAction(address);
            var plan = new ConstantUsers(user, usersCount);

            await new Scenario()
                .AddSequentialPlans(plan)
                .StartAsync();
        }

        public class UserAction : HttpUser
        {
            public UserAction(string address)
                : base(address) { }

            protected override async Task PerformanceAsync()
            {
                await Get("/posts");
            }
        }
    }
}
