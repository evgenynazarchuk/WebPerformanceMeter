using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebSocketWebApplication.IntegrationTest.Support;
using WebSocketWebApplication.IntegrationTest.Support.Tool;

namespace WebSocketWebApplication.IntegrationTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            // Arrange
            var env = new TestEnvironment();
            var buffer = WebSocket.CreateClientBuffer(1024, 1024);
            var wsClient = env.App.Server.CreateWebSocketClient();
            var wsUri = new UriBuilder(env.App.Server.BaseAddress)
            {
                Scheme = "ws",
                Path = "ws"
            }.Uri;

            var ws = await wsClient.ConnectAsync(wsUri, CancellationToken.None);

            // Act
            var receive = await ws.ReceiveAsync(buffer, CancellationToken.None);
            var welcomeMessage = Encoding.UTF8.GetString(buffer.ToArray(), 0, receive.Count);

            // Assert
            welcomeMessage.Should().Contain(" is now connected");
        }

        [TestMethod]
        public async Task TestMethod2()
        {
            // Arrange
            var env = new TestEnvironment();
            var message = "Hello world";
            var buffer = WebSocket.CreateClientBuffer(1024, 1024);
            var wsClient = env.App.Server.CreateWebSocketClient();
            var wsUri = new UriBuilder(env.App.Server.BaseAddress)
            {
                Scheme = "ws",
                Path = "ws"
            }.Uri;

            var ws1 = await wsClient.ConnectAsync(wsUri, CancellationToken.None);
            var ws2 = await wsClient.ConnectAsync(wsUri, CancellationToken.None);

            // Act
            await ws2.SendAsync(buffer: new ArraySegment<byte>(
                    array: Encoding.UTF8.GetBytes(message),
                    offset: 0,
                    count: message.Length),
                messageType: WebSocketMessageType.Text,
                endOfMessage: true,
                cancellationToken: CancellationToken.None);

            // read first
            await ws1.ReceiveAsync(buffer, CancellationToken.None);
            // read second
            await ws1.ReceiveAsync(buffer, CancellationToken.None);
            // read sent message
            var result = await ws1.ReceiveAsync(buffer, CancellationToken.None);
            var hello = Encoding.UTF8.GetString(buffer.ToArray(), 0, result.Count);

            // Assert
            hello.Should().Contain(" said: Hello world");
        }

        [TestMethod]
        public async Task TestMethod3()
        {
            // Arrange
            var wsUri = new UriBuilder()
            {
                Scheme = "ws",
                Host = "localhost",
                Port = 5000,
                Path = "ws"
            }.Uri;

            await using var client = new WebSocketClientTool(wsUri);
            await client.ConnectAsync();
            var message = await client.ReceiveMessageAsync();
            await client.DisconnectAsync();

            // Assert
            message.Should().Contain(" is now connected");
        }

        [TestMethod]
        public async Task TestMethod4()
        {
            // Arrange
            var wsUri = new UriBuilder()
            {
                Scheme = "ws",
                Host = "localhost",
                Port = 5000,
                Path = "ws"
            }.Uri;

            await using var client1 = new WebSocketClientTool(wsUri);
            await using var client2 = new WebSocketClientTool(wsUri);
            await using var client3 = new WebSocketClientTool(wsUri);

            // Act
            await client1.ConnectAsync();
            await client2.ConnectAsync();
            await client3.ConnectAsync();

            await client2.SendMessageAsync("Hello");
            await client3.SendMessageAsync("Hello");

            // Assert
            var message1 = await client1.ReceiveMessageAsync();
            var message2 = await client1.ReceiveMessageAsync();
            var message3 = await client1.ReceiveMessageAsync();
            var message4 = await client1.ReceiveMessageAsync();
            var message5 = await client1.ReceiveMessageAsync();

            Console.WriteLine(message1);
            Console.WriteLine(message2);
            Console.WriteLine(message3);
            Console.WriteLine(message4);
            Console.WriteLine(message5);
        }
    }
}
