using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebPerformanceMeter.Tools.GrpcTool
{
    public interface IGrpcClientTool
    {
        ValueTask<TResponse> UnaryCallAsync<TResponse, TRequest>(string methodCall, TRequest requestBody)
            where TRequest : class, new()
            where TResponse : class, new();

        ValueTask<TResponse> ClientStreamAsync<TResponse, TRequest>(string methodCall, ICollection<TRequest> requestBodyList)
            where TRequest : class, new()
            where TResponse : class, new();

        ValueTask<IReadOnlyCollection<TResponse>> ServerStreamAsync<TResponse, TRequest>(string methodCall, TRequest requestBody)
            where TRequest : class, new()
            where TResponse : class, new();

        ValueTask<IReadOnlyCollection<TResponse>> BidirectionalStreamAsync<TResponse, TRequest>(string methodCall, ICollection<TRequest> requestBodyList)
            where TRequest : class, new()
            where TResponse : class, new();
    }
}
