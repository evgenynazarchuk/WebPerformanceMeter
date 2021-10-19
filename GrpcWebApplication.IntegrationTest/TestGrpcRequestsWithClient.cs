using FluentAssertions;
using Google.Protobuf.WellKnownTypes;
using GrpcWebApplication.IntegrationTest.Support;
using GrpcWebApplication.Models;
using NUnit.Framework;
using System.Collections.Generic;
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
    }
}