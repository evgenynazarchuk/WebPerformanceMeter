using FluentAssertions;
using Google.Protobuf.WellKnownTypes;
using GrpcWebApplication.IntegrationTest.Support;
using GrpcWebApplication.IntegrationTest.Support.Tool;
using GrpcWebApplication.Models;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcWebApplication.IntegrationTest
{
    public class TestGrpcRequestsWithClient
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task UnaryCallAsyncTest()
        {
            // Arrange
            using var env = new TestEnvironment();
            using var httpClient = env.TestApplication.CreateDefaultClient();
            using var grpcClient = new GrpcClientTool(typeof(UserMessager.UserMessagerClient), httpClient);
            var text = "text text text";

            // Act
            var identity = await grpcClient.UnaryCallAsync<MessageIdentityDto, MessageCreateDto>("SendMessageAsync", new MessageCreateDto { Text = text });

            // Assert
            identity.Id.Should().NotBe(0);

            var resultMessage = env.Repository.Set<Message>().Find(identity.Id);
            resultMessage.Text.Should().Be(text);
        }

        [Test]
        public async Task ClientStreamCallAsyncTest()
        {
            // Arrange
            using var env = new TestEnvironment();
            using var httpClient = env.TestApplication.CreateDefaultClient();
            using var grpcClient = new GrpcClientTool(typeof(UserMessager.UserMessagerClient), httpClient);
            var messages = new List<MessageCreateDto>
            {
                new MessageCreateDto { Text = "test 1" },
                new MessageCreateDto { Text = "test 2" },
                new MessageCreateDto { Text = "test 3" }
            };

            // Act
            await grpcClient.ClientStreamAsync<Empty, MessageCreateDto>("SendMessages", messages);

            await Task.Delay(1000);

            // Assert
            var actualMessage = env.Repository.Set<Message>().ToList();
            actualMessage.Select(x => x.Text).Should().BeEquivalentTo("test 1", "test 2", "test 3");
        }

        [Test]
        public async Task ServerStreamCallTest()
        {
            // Arrange
            using var env = new TestEnvironment();
            using var httpClient = env.TestApplication.CreateDefaultClient();
            using var grpcClient = new GrpcClientTool(typeof(UserMessager.UserMessagerClient), httpClient);

            env.Repository.Set<Message>().Add(new Message { Text = "test 1" });
            env.Repository.Set<Message>().Add(new Message { Text = "test 2" });
            env.Repository.Set<Message>().Add(new Message { Text = "test 3" });
            env.Repository.SaveChanges();

            // Act
            var result = await grpcClient.ServerStreamAsync<MessageSimpleDto, Empty>("GetMessages", new Empty());

            // Arrange
            result.Select(x => x.Text).Should().BeEquivalentTo("test 1", "test 2", "test 3");
        }

        [Test]
        public async Task BidirectionalStreamCallTest()
        {
            // Arrange
            using var env = new TestEnvironment();
            using var httpClient = env.TestApplication.CreateDefaultClient();
            using var grpcClient = new GrpcClientTool(typeof(UserMessager.UserMessagerClient), httpClient);
            var messages = new List<MessageCreateDto>
            {
                new MessageCreateDto { Text = "test 1" },
                new MessageCreateDto { Text = "test 2" },
                new MessageCreateDto { Text = "test 3" },
            };

            // Act
            var result = await grpcClient.BidirectionalStreamAsync<MessageSimpleDto, MessageCreateDto>("Messages", messages);

            //await reader;

            // Arrange
            result.Select(x => x.Text).Should().BeEquivalentTo("test 1", "test 2", "test 3");
        }
    }
}