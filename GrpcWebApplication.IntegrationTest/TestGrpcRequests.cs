using FluentAssertions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcWebApplication.IntegrationTest.Support;
using GrpcWebApplication.Models;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcWebApplication.IntegrationTest
{
    public class Tests : TestEnvironment
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task SendMessage()
        {
            // Arrange
            var text = "text text text";

            // Act
            var identity = await this.UserMessagerClient.SendMessageAsync(new MessageCreateDto { Text = text });

            // Assert
            identity.Id.Should().NotBe(0);

            var resultMessage = this.Repository.Set<Message>().Find(identity.Id);
            resultMessage.Text.Should().Be(text);
        }

        [Test]
        public async Task SendMessages()
        {
            // Arrange

            // Act
            using var requestMethod = this.UserMessagerClient.SendMessages();
            await requestMethod.RequestStream.WriteAsync(new MessageCreateDto { Text = "test 1" });
            await requestMethod.RequestStream.WriteAsync(new MessageCreateDto { Text = "test 2" });
            await requestMethod.RequestStream.CompleteAsync();

            await Task.Delay(1000);

            // Assert
            var actualMessage = this.Repository.Set<Message>().ToList();
            actualMessage.Select(x => x.Text).Should().BeEquivalentTo("test 1", "test 2");
        }

        [Test]
        public void GetMessage()
        {
            // Arrange
            var message = this.Repository.Set<Message>().Add(new Message { Text = "test 1" });


            // Act
            var response = this.UserMessagerClient.GetMessage(new MessageIdentityDto { Id = message.Entity.Id });

            // Assert
            response.Text.Should().Be("test 1");
        }

        [Test]
        public async Task GetMessages()
        {
            // Arrange
            this.Repository.Set<Message>().Add(new Message { Text = "test 1" });
            this.Repository.Set<Message>().Add(new Message { Text = "test 2" });
            this.Repository.SaveChanges();
            List<string> expectedText = new();


            // Act
            using var call = this.UserMessagerClient.GetMessages(new Empty());
            while (await call.ResponseStream.MoveNext())
            {
                expectedText.Add(call.ResponseStream.Current.Text);
            }

            // Arrange
            expectedText.Should().BeEquivalentTo("test 1", "test 2");
        }

        [Test]
        public async Task Messages()
        {
            // Arrange
            var expectedText = new List<string>();

            // Act
            using var requestMethod = this.UserMessagerClient.Messages();

            await requestMethod.RequestStream.WriteAsync(new MessageCreateDto { Text = "test 1" });
            await requestMethod.RequestStream.WriteAsync(new MessageCreateDto { Text = "test 2" });

            await requestMethod.RequestStream.CompleteAsync();

            while (await requestMethod.ResponseStream.MoveNext())
            {
                expectedText.Add(requestMethod.ResponseStream.Current.Text);
            }

            //await reader;

            // Arrange
            expectedText.Should().BeEquivalentTo("test 1", "test 2");
        }
    }
}