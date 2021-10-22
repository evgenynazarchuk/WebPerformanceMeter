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
using FluentAssertions;
using WebPerformanceMeter.DataReader.CsvReader;

namespace PerformanceTests.Tests.JsonPlaceholder
{
    public class Demo10
    {
        [PerformanceTest(1)]
        public async Task ReadFromFileAndPostTest(int usersCount)
        {
            var address = "https://jsonplaceholder.typicode.com";
            var user = new UserAction(address);
            var reader = new JsonReader<PostDto>("Tests\\JsonPlaceholder\\Demo10_PostDto.json");
            var plan = new ConstantUsers<PostDto>(user, usersCount, reader);

            await new Scenario()
                .AddSequentialPlans(plan)
                .StartAsync();
        }

        public class UserAction : HttpUser<PostDto>
        {
            public UserAction(string address)
                : base(address) { }

            protected override async Task PerformanceAsync(PostDto data)
            {
                var createdPost = await PostAsJson<PostDto, PostDto>("/posts", data);

                createdPost?.Title.Should().Be(data.Title);
            }
        }
    }
}