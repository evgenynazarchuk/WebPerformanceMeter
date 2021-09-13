namespace GrpcWebApplication.IntegrationTest
{
    using System.Threading.Tasks;
    using NUnit.Framework;
    using GrpcWebApplication.IntegrationTest.Support;
    using Google.Protobuf.WellKnownTypes;
    using FluentAssertions;
    using Grpc.Net.Client;
    using Grpc.AspNetCore.Server;
    using Grpc.AspNetCore.ClientFactory;
    using Grpc.Net.ClientFactory;
    using Grpc.Core;
    using System.Collections.Generic;
    using System.Threading;
    using GrpcWebApplication.Models;
    using System.Linq;
    using GrpcWebApplication.Services;

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
        public async Task SendUnaryAndReadStream()
        {
            // Arrange
            var env = new TestEnvironment();

            // Act
            env.UserMessagerClient.SendMessage(new MessageRequest
            {
                Text = "test test 123"
            });

            using var call = env.UserMessagerClient.GetMessages(new Empty());

            // Assert
            while (await call.ResponseStream.MoveNext())
            {
                call.ResponseStream.Current.Text.Should().Be("test test 123");
            }
        }

        [Test]
        public async Task TestBiDirectionalStream()
        {
            // Arrange
            var expectedText = new List<string>();
            var env = new TestEnvironment();

            // Act
            using var call = env.UserMessagerClient.Messages();

            // Act 1
            var readStreamTask = Task.Run(async () =>
            {
                while (await call.ResponseStream.MoveNext())
                {
                    expectedText.Add(call.ResponseStream.Current.Text);
                }
            });

            // Act 2
            await call.RequestStream.WriteAsync(new MessageRequest { Text = "test test 1" });
            await call.RequestStream.WriteAsync(new MessageRequest { Text = "test test 2" });
            await call.RequestStream.CompleteAsync();

            // Assert
            await readStreamTask;
            expectedText.Should().BeEquivalentTo("test test 1", "test test 2");
        }
    }
}