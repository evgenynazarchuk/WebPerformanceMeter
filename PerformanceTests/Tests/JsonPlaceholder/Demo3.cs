﻿using FluentAssertions;
using PerformanceTests.Tests.JsonPlaceholder.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebPerformanceMeter;
using WebPerformanceMeter.Attributes;
using WebPerformanceMeter.Support;

namespace PerformanceTests.Tests.JsonPlaceholder
{
    [PerformanceClass]
    public class Demo3
    {
        [PerformanceTest(5)]
        public async Task GetAllPostsTest(int usersCount)
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
                var posts = await GetAsJson<List<PostDto>>("/posts");

                posts?.Count.Should().Be(100);
            }
        }
    }
}
