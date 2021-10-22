using FluentAssertions;
using PerformanceTests.Tests.JsonPlaceholder.Dto;
using System.Threading.Tasks;
using WebPerformanceMeter;
using WebPerformanceMeter.Attributes;
using WebPerformanceMeter.Support;

namespace PerformanceTests.Tests.JsonPlaceholder
{
    public class Demo2
    {
        [PerformanceTest(5)]
        public async Task GetUserByIdTest(int usersCount)
        {
            var address = "https://jsonplaceholder.typicode.com";
            var user = new UserAction(address);
            var plan = new ConstantUsers(user, usersCount);

            await new Scenario()
                .AddSequentialPlans(plan)
                .Start();
        }

        public class UserAction : HttpUser
        {
            public UserAction(string address)
                : base(address) { }

            protected override async Task Performance()
            {
                var user = await GetAsJson<UserDto>("/users/1");

                user?.Name.Should().Be("Leanne Graham");
                user?.Address?.City.Should().Be("Gwenborough");
                user?.Company?.Name.Should().Be("Romaguera-Crona");
            }
        }
    }
}
