﻿using System.Net.Http;
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
