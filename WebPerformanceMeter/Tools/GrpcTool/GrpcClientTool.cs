using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebPerformanceMeter.Tools.GrpcTool
{
    public sealed class GrpcClientTool : IGrpcClientTool, IDisposable
    {
        private readonly GrpcChannel _grpcChannel;

        private readonly object _grpcClient;

        public GrpcClientTool(string address, Type serviceClientType)
        {
            this._grpcChannel = GrpcChannel.ForAddress(address);

            var ctor = serviceClientType.GetConstructor(new[] { typeof(GrpcChannel) });

            this._grpcClient = ctor is not null 
                ? ctor.Invoke(new[] { this._grpcChannel }) 
                : throw new ApplicationException("gRpc client is not create");

            if (this._grpcClient is null)
            {
                throw new ApplicationException("gRpc client is not create");
            }
        }

        public GrpcClientTool(HttpClient httpClient, Type serviceClientType)
        {
            this._grpcChannel = httpClient.BaseAddress is not null
                ? GrpcChannel.ForAddress(httpClient.BaseAddress, new GrpcChannelOptions { HttpClient = httpClient })
                : throw new ApplicationException("Base address is not set");

            var ctor = serviceClientType.GetConstructor(new[] { typeof(GrpcChannel) });

            this._grpcClient = ctor is not null
                ? ctor.Invoke(new[] { this._grpcChannel })
                : throw new ApplicationException("gRpc client is not create");
        }

        public static GrpcClientTool Create(string address, Type serviceClientType)
            => new GrpcClientTool(address, serviceClientType);

        public async ValueTask<TResponse> UnaryCallAsync<TResponse, TRequest>(string methodCall, TRequest requestBody)
            where TRequest : class, new()
            where TResponse : class, new()
        {
            var method = this._grpcClient
                .GetType()
                .GetMethods()
                .Where(x => x.Name == methodCall)
                .Single(x => x.GetParameters().Count() == 4);

            // TODO logging
            var response = await (AsyncUnaryCall<TResponse>)method.Invoke(this._grpcClient, new object[] { requestBody, null, null, null });
            //

            return response;
        }

        public async ValueTask<TResponse> ClientStreamAsync<TResponse, TRequest>(string methodCall, ICollection<TRequest> requestBodyList)
            where TRequest : class, new()
            where TResponse : class, new()
        {
            var method = this._grpcClient
                .GetType()
                .GetMethods()
                .Where(x => x.Name == methodCall)
                .Single(x => x.GetParameters().Count() == 3);

            using var grpcConnect = (AsyncClientStreamingCall<TRequest, TResponse>)method.Invoke(this._grpcClient, new object[] { null, null, null });

            // TODO logging
            foreach (var requestBody in requestBodyList)
            {
                await grpcConnect.RequestStream.WriteAsync(requestBody);
            }

            await grpcConnect.RequestStream.CompleteAsync();

            return grpcConnect.ResponseAsync.Result;
        }

        public async ValueTask<IReadOnlyCollection<TResponse>> ServerStreamAsync<TResponse, TRequest>(string methodCall, TRequest requestBody)
            where TRequest : class, new()
            where TResponse : class, new()
        {
            var method = this._grpcClient
                .GetType()
                .GetMethods()
                .Where(x => x.Name == methodCall)
                .Single(x => x.GetParameters().Count() == 4);

            using var grpcConnect = (AsyncServerStreamingCall<TResponse>)method.Invoke(this._grpcClient, new object[] { requestBody, null, null, null });
            var messages = new List<TResponse>();

            // TODO logging
            while (await grpcConnect.ResponseStream.MoveNext())
            {
                messages.Add(grpcConnect.ResponseStream.Current);
            }

            return messages;
        }

        public async ValueTask<IReadOnlyCollection<TResponse>> BidirectionalStreamAsync<TResponse, TRequest>(string methodCall, ICollection<TRequest> requestBodyList)
            where TRequest : class, new()
            where TResponse : class, new()
        {
            var method = this._grpcClient
                .GetType()
                .GetMethods()
                .Where(x => x.Name == methodCall)
                .Single(x => x.GetParameters().Count() == 3);

            using var grpcConnect = (AsyncDuplexStreamingCall<TRequest, TResponse>)method.Invoke(this._grpcClient, new object[] { null, null, null });
            var responseMessages = new List<TResponse>();

            var readMessageTask = Task.Run(async () =>
            {
                await foreach (var responseMessage in grpcConnect.ResponseStream.ReadAllAsync())
                {
                    responseMessages.Add(responseMessage);
                }
            });

            foreach (var requestBody in requestBodyList)
            {
                await grpcConnect.RequestStream.WriteAsync(requestBody);
            }

            await grpcConnect.RequestStream.CompleteAsync();
            await readMessageTask;

            return responseMessages;
        }

        public void Dispose()
        {
            this._grpcChannel.Dispose();
        }
    }
}
