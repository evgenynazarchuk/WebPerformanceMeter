using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebPerformanceMeter.Tools.GrpcTool;

namespace WebPerformanceMeter.Users.Grpc
{
    public interface IGrpcUser : IUser
    {
        void UseGrpcClient(Type grpcClient);

        ValueTask<TResponse> UnaryCallAsync<TResponse, TRequest>(
            GrpcClientTool client,
            string methodCall,
            TRequest requestBody,
            string label = "")
            where TRequest : class, new()
            where TResponse : class, new();

        ValueTask<TResponse> ClientStreamAsync<TResponse, TRequest>(
            GrpcClientTool client,
            string methodCall,
            ICollection<TRequest> requestBodyList,
            string label = "")
            where TRequest : class, new()
            where TResponse : class, new();

        ValueTask<IReadOnlyCollection<TResponse>> ServerStreamAsync<TResponse, TRequest>(
            GrpcClientTool client,
            string methodCall,
            TRequest requestBody,
            string label = "")
            where TRequest : class, new()
            where TResponse : class, new();

        ValueTask<IReadOnlyCollection<TResponse>> BidirectionalStreamAsync<TResponse, TRequest>(
            GrpcClientTool client,
            string methodCall,
            ICollection<TRequest> requestBodyList,
            string label = "")
            where TRequest : class, new()
            where TResponse : class, new();
    }
}
