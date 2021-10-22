using PerformanceTests.Tests.JsonPlaceholder.Dto;
using System.Threading.Tasks;
using WebPerformanceMeter;
using WebPerformanceMeter.Attributes;
using WebPerformanceMeter.Support;

namespace PerformanceTests.Tests.JsonPlaceholder
{
    public class Demo4
    {
        [PerformanceTest(1)]
        public async Task CreatePostsWithoutResultTest(int usersCount)
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
                var post = new PostDto { UserId = 1, Title = "test", Body = "test" };
                await PostAsJson<PostDto>("/posts", post);
            }
        }
    }
}
