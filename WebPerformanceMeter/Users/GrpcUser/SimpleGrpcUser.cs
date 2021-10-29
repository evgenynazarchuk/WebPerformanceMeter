using System;
using System.Net.Http;
using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Users;

namespace WebPerformanceMeter
{
    public abstract partial class GrpcUser : BasicGrpcUser, ISimpleUser
    {
        public GrpcUser(string address, Type grpcClientType, string? userName = null)
            : base(address, grpcClientType, userName ?? typeof(GrpcUser).Name)
        {
            //this.httpClient = new HttpClient();
            //this.httpClient.BaseAddress = new Uri(address);
            //this.httpClient.Timeout = TimeSpan.FromMinutes(2);
            ////this.channel = GrpcChannel.ForAddress(httpClient.BaseAddress, new GrpcChannelOptions
            ////{
            ////    HttpClient = this.httpClient,
            ////});

            //this.GrpcClientTool = new GrpcClientTool(httpClient, grpcClientType, this.Watcher);
        }

        public virtual async Task InvokeAsync(int loopCount = 1)
        {
            //if (this.grpcClientType is null)
            //{
            //    throw new ApplicationException("Grpc Client Type is not set. Try UseGrpcClient()");
            //}
        
            //using var client = new GrpcClientTool(this.httpClient, this.grpcClientType, this.Watcher);
        
            for (int i = 0; i < loopCount; i++)
            {
                await PerformanceAsync();
            }
        }

        protected abstract Task PerformanceAsync();

        //protected readonly HttpClient httpClient;

        //protected GrpcChannel channel;
    }
}
