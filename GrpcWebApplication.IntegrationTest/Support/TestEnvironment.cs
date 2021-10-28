using Grpc.Net.Client;
using GrpcWebApplication.IntegrationTest.Support.Tool;
using GrpcWebApplication.Services;
using System;
using System.Net.Http;

namespace GrpcWebApplication.IntegrationTest.Support
{
    public class TestEnvironment : IDisposable
    {
        public readonly TestApplication App;

        public readonly HttpClient HttpClient;

        public readonly GrpcChannel GrpcChannel;

        public readonly UserMessagerService.UserMessagerServiceClient UserMessagerClient;

        public readonly GrpcClientTool GrpcClient;

        public readonly DataContext Repository;

        public TestEnvironment()
        {
            this.App = new();

            this.HttpClient = this.App.CreateDefaultClient();
            if (this.HttpClient.BaseAddress is null)
            {
                throw new ApplicationException("address is not set");
            }

            this.GrpcChannel = GrpcChannel.ForAddress(this.HttpClient.BaseAddress, new GrpcChannelOptions { HttpClient = this.HttpClient });
            this.UserMessagerClient = new UserMessagerService.UserMessagerServiceClient(this.GrpcChannel);
            this.GrpcClient = new GrpcClientTool(this.HttpClient, typeof(UserMessagerService.UserMessagerServiceClient));

            this.Repository = new DataContext();
            this.Repository.Database.EnsureCreated();
        }

        public void Dispose()
        {
            this.GrpcChannel.Dispose();
            this.Repository.Database.EnsureDeleted();
        }
    }
}
