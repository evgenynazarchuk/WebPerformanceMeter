using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebSocketWebApplication.IntegrationTest.Support.Tool;

namespace WebSocketWebApplication.IntegrationTest
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            // Arrange
            var wsUri = new UriBuilder()
            {
                Host = "localhost",
                Scheme = "ws",
                Path = "ws",
                Port = 5000
            }.Uri;
            var client = new WebSocketClientTool(wsUri);

            // Act
            await client.ConnectAsync();
            var message = await client.ReceiveMessageAsync();
            await client.DisconnectAsync();

            // Assert
            Console.WriteLine(message);
        }

        [TestMethod]
        public async Task TestMethod2()
        {
            // Arrange
            var wsUri = new UriBuilder()
            {
                Host = "localhost",
                Scheme = "ws",
                Path = "ws",
                Port = 5000
            }.Uri;
            var client1 = new WebSocketClientTool(wsUri);
            var client2 = new WebSocketClientTool(wsUri);

            // Act
            await client1.ConnectAsync();
            await client2.ConnectAsync();

            var message1 = await client1.ReceiveMessageAsync();
            var message2 = await client2.ReceiveMessageAsync();

            await client1.DisconnectAsync();
            await client2.DisconnectAsync();

            // Assert
            Console.WriteLine(message1);
            Console.WriteLine(message2);
        }

        [TestMethod]
        public async Task TestMethod3()
        {
            // Arrange
            var wsUri = new UriBuilder()
            {
                Host = "localhost",
                Scheme = "ws",
                Path = "ws",
                Port = 5000
            }.Uri;
            var client1 = new WebSocketClientTool(wsUri);
            var client2 = new WebSocketClientTool(wsUri);

            // Act
            await client1.ConnectAsync();
            await client2.ConnectAsync();

            var message1_1 = await client1.ReceiveMessageAsync();
            var message1_2 = await client1.ReceiveMessageAsync();

            var message2 = await client2.ReceiveMessageAsync();

            await client1.DisconnectAsync();
            await client2.DisconnectAsync();

            // Assert
            Console.WriteLine(message1_1);
            Console.WriteLine(message1_2);
            Console.WriteLine(message2);
        }

        [TestMethod]
        public async Task TestMethod4()
        {
            // Arrange
            var wsUri = new UriBuilder()
            {
                Host = "localhost",
                Scheme = "ws",
                Path = "ws",
                Port = 5000
            }.Uri;
            var client1 = new WebSocketClientTool(wsUri);
            var client2 = new WebSocketClientTool(wsUri);

            // Act
            await client1.ConnectAsync();
            await client2.ConnectAsync();

            var message1_1 = await client1.ReceiveMessageAsync();
            var message1_2 = await client1.ReceiveMessageAsync();

            await client1.DisconnectAsync();

            var message2 = await client2.ReceiveMessageAsync();

            await client2.DisconnectAsync();

            // Assert
            Console.WriteLine(message1_1);
            Console.WriteLine(message1_2);
            Console.WriteLine(message2);
        }

        [TestMethod]
        public void TestMethod5()
        {
            // Arrange
            var wsUri = new UriBuilder()
            {
                Host = "localhost",
                Scheme = "ws",
                Path = "ws",
                Port = 5000
            }.Uri;
            var tasks = new List<Task>();

            for (int i = 0; i < 1000; i++)
            {
                var task = Task.Run(async () =>
                {
                    DateTime startConnect;
                    DateTime endConnect;
                    DateTime startReceive;
                    DateTime endReceive;

                    var client = new WebSocketClientTool(wsUri);

                    startConnect = DateTime.UtcNow;
                    await client.ConnectAsync();
                    endConnect = DateTime.UtcNow;

                    startReceive = DateTime.UtcNow;
                    var message = await client.ReceiveMessageAsync();
                    endReceive = DateTime.UtcNow;

                    Console.WriteLine($"{endConnect - startConnect} {endReceive - startReceive}");

                    await client.DisconnectAsync();
                });

                tasks.Add(task);
            }

            // Assert
            Task.WaitAll(tasks.ToArray());
        }
    }
}
