namespace GrpcWebApplication.IntegrationTest.Support
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Grpc.Net.Client;
    using System.Net.Http;

    public class TestEnvironment
    {
        public readonly TestApplication TestApplication;

        public readonly HttpClient HttpClient;

        public readonly GrpcChannel GrpcChannel;

        public readonly UserMessager.UserMessagerClient UserMessagerClient;

        public TestEnvironment()
        {
            this.TestApplication = new();
            this.HttpClient = this.TestApplication.CreateDefaultClient();
            this.GrpcChannel = GrpcChannel.ForAddress(this.HttpClient.BaseAddress, new GrpcChannelOptions
            {
                HttpClient = this.HttpClient
            });
            this.UserMessagerClient = new UserMessager.UserMessagerClient(this.GrpcChannel);
        }
    }
}
