using System;
using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Logger;
using WebPerformanceMeter.Tools.GrpcTool;

namespace WebPerformanceMeter.Users.Grpc
{
    public abstract partial class GrpcUser : User, IGrpcUser
    {
        protected readonly string address;

        protected Type? grpcClientType = null;

        public GrpcUser(string address, ILogger? logger = null, string userName = "")
            : base(logger ?? GrpcLoggerSingleton.GetInstance())
        {
            this.SetUserName(string.IsNullOrEmpty(userName) ? this.GetType().Name : userName);
            this.address = address;
        }

        public virtual void UseGrpcClient(Type grpcClient)
        {
            this.grpcClientType = grpcClient;
        }

        public override async Task InvokeAsync(
            int loopCount = 1,
            IDataReader? dataReader = null,
            bool reuseDataInLoop = true
            )
        {
            if (grpcClientType is null)
            {
                throw new ApplicationException("Grpc Client Type is not set. Try UseGrpcClient()");
            }

            using var client = new GrpcClientTool(this.address, this.Logger, this.grpcClientType.GetType());

            object? entity = null;

            if (dataReader is not null)
            {
                entity = dataReader.GetEntity();

                if (entity is null)
                {
                    return;
                }
            }

            for (int i = 0; i < loopCount; i++)
            {
                if (entity is null)
                {
                    await PerformanceAsync(client);
                }
                else
                {
                    await PerformanceAsync(client, entity);
                }

                if (dataReader is not null && !reuseDataInLoop)
                {
                    entity = dataReader.GetEntity();

                    if (entity is null)
                    {
                        return;
                    }
                }
            }
        }

        protected virtual Task PerformanceAsync(GrpcClientTool client, object entity)
        {
            return Task.CompletedTask;
        }

        protected virtual Task PerformanceAsync(GrpcClientTool client)
        {
            return Task.CompletedTask;
        }
    }
}
