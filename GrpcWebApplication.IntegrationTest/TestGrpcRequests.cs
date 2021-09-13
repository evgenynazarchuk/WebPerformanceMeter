namespace GrpcWebApplication.IntegrationTest
{
    using FluentAssertions;
    using Google.Protobuf.WellKnownTypes;
    using Grpc.Core;
    using GrpcWebApplication.IntegrationTest.Support;
    using GrpcWebApplication.Models;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task GetMessage()
        {
            // Arrange
            using var env = new TestEnvironment();
            env.Repository.Set<Message>().Add(new Message
            {
                Text = "test 1"
            });
            env.Repository.SaveChanges();

            // Act
            using var call = env.UserMessagerClient.GetMessages(new Empty());
            await call.ResponseStream.MoveNext();
            var result = call.ResponseStream.Current.Text;

            // Assert
            result.Should().Be("test 1");
        }

        [Test]
        public async Task GetMessages()
        {
            // Arrange
            using var env = new TestEnvironment();
            List<string> expectedText = new();
            env.Repository.Set<Message>().Add(new Message
            {
                Text = "test 1"
            });
            env.Repository.Set<Message>().Add(new Message
            {
                Text = "test 2"
            });
            env.Repository.SaveChanges();


            // Act
            using var call = env.UserMessagerClient.GetMessages(new Empty());
            while (await call.ResponseStream.MoveNext())
            {
                expectedText.Add(call.ResponseStream.Current.Text);
            }

            // Arrange
            expectedText.Should().BeEquivalentTo("test 1", "test 2");
        }

        [Test]
        public async Task SendMessage()
        {
            // Arrange
            using var env = new TestEnvironment();

            // Act
            await env.UserMessagerClient.SendMessageAsync(new MessageRequest { Text = "test 1" });

            // Assert
            var actualMessage = env.Repository.Set<Message>().Single();
            actualMessage.Text.Should().Be("test 1");
        }

        [Test]
        public async Task SendMessages()
        {
            // Arrange
            using var env = new TestEnvironment();

            // Act
            using var requestCall = env.UserMessagerClient.SendMessages();
            await requestCall.RequestStream.WriteAsync(new MessageRequest { Text = "test 1" });
            await requestCall.RequestStream.WriteAsync(new MessageRequest { Text = "test 2" });
            await requestCall.RequestStream.CompleteAsync();

            await Task.Delay(1000);

            // Assert
            var actualMessage = env.Repository.Set<Message>().ToList();
            actualMessage.Select(x => x.Text).Should().BeEquivalentTo("test 1", "test 2");
        }

        [Test]
        public async Task Messages()
        {
            // Arrange
            var expectedText = new List<string>();
            using var env = new TestEnvironment();

            // Act
            using var call = env.UserMessagerClient.Messages();

            var reader = Task.Run(async () =>
            {
                while (await call.ResponseStream.MoveNext())
                {
                    expectedText.Add(call.ResponseStream.Current.Text);
                }
            });

            await call.RequestStream.WriteAsync(new MessageRequest { Text = "test 1" });
            await call.RequestStream.WriteAsync(new MessageRequest { Text = "test 2" });

            await call.RequestStream.CompleteAsync();
            await reader;

            // Arrange
            expectedText.Should().BeEquivalentTo("test 1", "test 2");
        }
    }
}