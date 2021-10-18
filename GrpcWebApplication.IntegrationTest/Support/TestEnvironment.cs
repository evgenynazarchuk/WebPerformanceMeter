﻿using Grpc.Net.Client;
using GrpcWebApplication.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace GrpcWebApplication.IntegrationTest.Support
{
    public class TestEnvironment : IDisposable
    {
        public readonly TestApplication TestApplication;

        public readonly HttpClient HttpClient;

        public readonly GrpcChannel GrpcChannel;

        public readonly UserMessager.UserMessagerClient UserMessagerClient;

        public readonly DataContext Repository;

        public TestEnvironment()
        {
            this.TestApplication = new();

            this.HttpClient = this.TestApplication.CreateDefaultClient();
            this.GrpcChannel = GrpcChannel.ForAddress(this.HttpClient.BaseAddress, new GrpcChannelOptions
            {
                HttpClient = this.HttpClient
            });
            this.UserMessagerClient = new UserMessager.UserMessagerClient(this.GrpcChannel);

            this.Repository = new DataContext();
            this.Repository.Database.EnsureCreated();
        }

        public void Dispose()
        {
            this.Repository.Database.EnsureDeleted();
        }
    }
}
