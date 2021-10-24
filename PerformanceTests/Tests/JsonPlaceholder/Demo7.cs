using FluentAssertions;
using PerformanceTests.Tests.JsonPlaceholder.Dto;
using System.Threading.Tasks;
using WebPerformanceMeter;
using WebPerformanceMeter.Attributes;
using WebPerformanceMeter.Support;

namespace PerformanceTests.Tests.JsonPlaceholder
{
    [PerformanceClass]
    public class Demo7
    {
        [PerformanceTest(1)]
        public async Task UpdatePostsWithResultTest(int usersCount)
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
                var post = new PostDto { Title = "test", Body = "test" };
                var updatedPost = await PutAsJson<PostDto, PostDto>("/posts/1", post);

                updatedPost?.Title.Should().Be("test");
            }
        }
    }
}
