using System;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Logger;

namespace WebPerformanceMeter.Users
{
    public abstract partial class BasicGrpcUser : BaseUser
    {
        protected readonly string address;

        protected Type? grpcClientType = null;

        public BasicGrpcUser(string address, string? userName = null, ILogger? logger = null)
            : base(userName ?? typeof(BasicGrpcUser).Name, logger ?? GrpcLoggerSingleton.GetInstance())
        {
            this.address = address;
        }

        public virtual void UseGrpcClient(Type grpcClient)
        {
            this.grpcClientType = grpcClient;
        }
    }
}
