using FluentAssertions;
using PerformanceTests.Tests.JsonPlaceholder.Dto;
using System.Threading.Tasks;
using WebPerformanceMeter;
using WebPerformanceMeter.Attributes;
using WebPerformanceMeter.DataReader.CsvReader;
using WebPerformanceMeter.Support;

namespace PerformanceTests.Tests.JsonPlaceholder
{
    [PerformanceClass]
    public class Demo10
    {
        private readonly string _address = "https://jsonplaceholder.typicode.com";

        [PerformanceTest(3, "test project", "123456789")]
        public async Task ReadFromFileAndPostTest(int usersCount, string projectName, string testRunId)
        {
            var user = new UserAction(this._address);
            var reader = new JsonReader<PostDto>("Tests\\JsonPlaceholder\\Demo10_PostDto.json", cyclicalData: true);
            var plan = new ConstantUsers<PostDto>(user, usersCount, reader);

            await new Scenario(projectName, testRunId)
                .AddSequentialPlans(plan)
                .Start();
        }

        public class UserAction : HttpUser<PostDto>
        {
            public UserAction(string address)
                : base(address) { }

            protected override async Task Performance(PostDto data)
            {
                var createdPost = await PostAsJson<PostDto, PostDto>("/posts", data);

                createdPost?.Title.Should().Be(data.Title);
            }
        }
    }
}