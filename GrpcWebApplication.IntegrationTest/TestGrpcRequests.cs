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

    public class Tests
    {
        [SetUp]
        public void Setup()
        {
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
            while (await call.ResponseStream.MoveNext(new System.Threading.CancellationToken()))
            {
                call.ResponseStream.Current.Text.Should().Be("test test 123");
            }
        }

        [Test]
        public async Task SendStreamAndReadStream()
        {
            // Arrange
            var env = new TestEnvironment();
            List<string> expectedText = new();

            // Act
            using var requestCall = env.UserMessagerClient.SendMessages();
            await requestCall.RequestStream.WriteAsync(new MessageRequest { Text = "test test 1" });
            await requestCall.RequestStream.WriteAsync(new MessageRequest { Text = "test test 2" });
            await requestCall.RequestStream.CompleteAsync();

            using var responseCall = env.UserMessagerClient.GetMessages(new Empty());
            while (await responseCall.ResponseStream.MoveNext(new System.Threading.CancellationToken()))
            {
                expectedText.Add(responseCall.ResponseStream.Current.Text);
            }

            // Assert
            expectedText.Should().BeEquivalentTo("test test 1", "test test 2");
        }

        [Test]
        public async Task TestBiDirectionalStream()
        {
            // Arrange
            var env = new TestEnvironment();
            List<string> expectedText = new();

            // Act
            using var call = env.UserMessagerClient.Messages();

            await call.RequestStream.WriteAsync(new MessageRequest { Text = "test test 1" });
            await call.RequestStream.WriteAsync(new MessageRequest { Text = "test test 2" });
            await call.RequestStream.CompleteAsync();

            await foreach (var response in call.ResponseStream.ReadAllAsync())
            {
                expectedText.Add(response.Text);
            }
            //while (await call.ResponseStream.MoveNext(new System.Threading.CancellationToken()))
            //{
            //    expectedText.Add(call.ResponseStream.Current.Text);
            //}

            // Assert
            expectedText.Should().BeEquivalentTo("test test 1", "test test 2");
        }
    }
}