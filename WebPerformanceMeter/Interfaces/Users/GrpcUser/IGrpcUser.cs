using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;

namespace WebPerformanceMeter.Interfaces
{
    public interface IGrpcUser : IBaseUser
    {
        void UseGrpcClient(Type grpcClient);

        ValueTask<TResponse> UnaryCall<TResponse, TRequest>(
            GrpcClientTool client,
            string methodCall,
            TRequest requestBody,
            string label = "")
            where TRequest : class, new()
            where TResponse : class, new();

        ValueTask<TResponse> ClientStream<TResponse, TRequest>(
            GrpcClientTool client,
            string methodCall,
            ICollection<TRequest> requestBodyList,
            string label = "")
            where TRequest : class, new()
            where TResponse : class, new();

        ValueTask<IReadOnlyCollection<TResponse>> ServerStream<TResponse, TRequest>(
            GrpcClientTool client,
            string methodCall,
            TRequest requestBody,
            string label = "")
            where TRequest : class, new()
            where TResponse : class, new();

        ValueTask<IReadOnlyCollection<TResponse>> BidirectionalStream<TResponse, TRequest>(
            GrpcClientTool client,
            string methodCall,
            ICollection<TRequest> requestBodyList,
            string label = "")
            where TRequest : class, new()
            where TResponse : class, new();
    }
}
