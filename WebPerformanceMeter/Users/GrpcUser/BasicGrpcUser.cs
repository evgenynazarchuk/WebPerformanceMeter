using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebPerformanceMeter.Users
{
    public abstract partial class BasicGrpcUser : BasicUser
    {
        public BasicGrpcUser(string address, Type grpcClientType, string userName)
            : base(userName)
        {
            this.address = address;

            this.httpClient = new();
            this.httpClient.BaseAddress = new(address);
            this.GrpcClientTool = new(httpClient, grpcClientType, this.Watcher);
        }

        public void UseGrpcClient(Type grpcClient)
        {
            this.grpcClientType = grpcClient;
        }

        //protected abstract Task PerformanceAsync(GrpcClientTool client);

        //protected abstract Task PerformanceAsync();

        protected readonly string address;

        protected readonly HttpClient httpClient;

        protected readonly GrpcClientTool GrpcClientTool;

        protected Type? grpcClientType = null;

        
    }
}
