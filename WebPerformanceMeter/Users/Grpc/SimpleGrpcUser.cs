using System;
using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Users;

namespace WebPerformanceMeter
{
    public abstract class GrpcUser : BasicGrpcUser, ISimpleUser
    {

        public GrpcUser(string address, string? userName = null, ILogger? logger = null)
            : base(address, userName, logger) { }

        public virtual async Task InvokeAsync(int loopCount = 1)
        {
            if (this.grpcClientType is null)
            {
                throw new ApplicationException("Grpc Client Type is not set. Try UseGrpcClient()");
            }

            using var client = new GrpcClientTool(this.address, this.grpcClientType.GetType(), this.logger);

            for (int i = 0; i < loopCount; i++)
            {
                await PerformanceAsync(client);
            }
        }

        protected abstract Task PerformanceAsync(GrpcClientTool client);
    }
}
