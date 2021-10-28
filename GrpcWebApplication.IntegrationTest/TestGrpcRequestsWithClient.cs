using FluentAssertions;
using Google.Protobuf.WellKnownTypes;
using GrpcWebApplication.IntegrationTest.Support;
using GrpcWebApplication.IntegrationTest.Support.Tool;
using GrpcWebApplication.Models;
using NUnit.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcWebApplication.IntegrationTest
{
    public class TestGrpcRequestsWithClient : TestEnvironment
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task UnaryCallAsyncTest()
        {
            // Arrange
            var text = "text text text";

            // Act
            var identity = await this.GrpcClient.UnaryCallAsync<MessageIdentityDto, MessageCreateDto>("SendMessageAsync", new MessageCreateDto { Text = text });

            // Assert
            identity.Id.Should().NotBe(0);

            var resultMessage = this.Repository.Set<Message>().Find(identity.Id);
            resultMessage.Text.Should().Be(text);
        }

        [Test]
        public async Task ClientStreamCallAsyncTest()
        {
            // Arrange
            var messages = new List<MessageCreateDto>
            {
                new MessageCreateDto { Text = "test 1" },
                new MessageCreateDto { Text = "test 2" },
                new MessageCreateDto { Text = "test 3" }
            };

            // Act
            await this.GrpcClient.ClientStreamAsync<Empty, MessageCreateDto>("SendMessages", messages);

            await Task.Delay(1000);

            // Assert
            var actualMessage = this.Repository.Set<Message>().ToList();
            actualMessage.Select(x => x.Text).Should().BeEquivalentTo("test 1", "test 2", "test 3");
        }

        [Test]
        public async Task ServerStreamCallTest()
        {
            // Arrange
            this.Repository.Set<Message>().Add(new Message { Text = "test 1" });
            this.Repository.Set<Message>().Add(new Message { Text = "test 2" });
            this.Repository.Set<Message>().Add(new Message { Text = "test 3" });
            this.Repository.SaveChanges();

            // Act
            var result = await this.GrpcClient.ServerStreamAsync<MessageSimpleDto, Empty>("GetMessages", new Empty());

            // Arrange
            result.Select(x => x.Text).Should().BeEquivalentTo("test 1", "test 2", "test 3");
        }

        [Test]
        public async Task BidirectionalStreamCallTest()
        {
            // Arrange
            var messages = new List<MessageCreateDto>
            {
                new MessageCreateDto { Text = "test 1" },
                new MessageCreateDto { Text = "test 2" },
                new MessageCreateDto { Text = "test 3" },
            };

            // Act
            var result = await this.GrpcClient.BidirectionalStreamAsync<MessageSimpleDto, MessageCreateDto>("Messages", messages);

            //await reader;

            // Arrange
            result.Select(x => x.Text).Should().BeEquivalentTo("test 1", "test 2", "test 3");
        }

        [Test]
        public async Task ManyUnaryCallTest()
        {
            // Arrange
            int clientCount = 100;
            var watcher = new Stopwatch();
            var requestTasks = new ConcurrentBag<ValueTask<MessageIdentityDto>>();
            var clients = new List<GrpcClientTool>();

            TimeSpan t1 = default,
                t2 = default,
                t3 = default,
                t4 = default;
            //t5 = default,
            //t6 = default;

            // Act
            watcher.Start();

            t1 = watcher.Elapsed;
            for (int i = 0; i < clientCount; i++)
            {
                var grpcClient = new GrpcClientTool("https://localhost:5001", typeof(UserMessagerService.UserMessagerServiceClient));
                clients.Add(grpcClient);
            }
            t2 = watcher.Elapsed;
            Console.WriteLine($"client create time: {t1} {t2} {t2 - t1}");

            var requestCalltTasks = new List<Task>();
            var requestsTime = new List<string>();
            for (int i = 0; i < clientCount; i++)
            {
                var requestCallTask = Task.Factory.StartNew((i) =>
                {
                    var index = Convert.ToInt32(i);
                    t3 = watcher.Elapsed;
                    var task = clients[index].UnaryCallAsync<MessageIdentityDto, MessageCreateDto>("SendMessageAsync", new MessageCreateDto { Text = "test" });
                    t4 = watcher.Elapsed;

                    requestTasks.Add(task);
                    //requestsTime.Add($"request call time: {t3} {t4} {t4 - t3}");
                    Console.WriteLine($"request call time: {t3} {t4} {t4 - t3}");
                }, i);

                requestCalltTasks.Add(requestCallTask);
            }
            await Task.WhenAll(requestCalltTasks);
            //Console.WriteLine(String.Join("\n", requestsTime));


            var waitTasks = new List<Task>();
            var requestsWaitTime = new ConcurrentBag<string>();
            foreach (var task in requestTasks)
            {
                var waitTask = Task.Run(async () =>
                {
                    var t5 = watcher.Elapsed;
                    await task;
                    var t6 = watcher.Elapsed;

                    //requestsWaitTime.Add($"request wait time: {t5} {t6} {t6 - t5}");
                    Console.WriteLine($"request wait time: {t5} {t6} {t6 - t5}");
                });

                waitTasks.Add(waitTask);
            }
            await Task.WhenAll(waitTasks);
            //Console.WriteLine(String.Join("\n", waitTasks));
            watcher.Stop();

            // Arrange
            Console.WriteLine($"result: {watcher.Elapsed}");
        }
    }
}