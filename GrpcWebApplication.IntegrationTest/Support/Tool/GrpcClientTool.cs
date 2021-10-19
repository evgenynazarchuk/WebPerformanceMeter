using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GrpcWebApplication.IntegrationTest.Support.Tool
{
    public class GrpcClientTool : IGrpcClientTool, IDisposable
    {
        protected readonly GrpcChannel _grpcChannel;

        protected readonly object _grpcClient;

        public GrpcClientTool(Type grpcClient, string address)
        {
            this._grpcChannel = GrpcChannel.ForAddress(address);
            this._grpcClient = Activator.CreateInstance(type: grpcClient, args: this._grpcChannel);
        }

        public GrpcClientTool(Type grpcClient, HttpClient httpClient)
        {
            this._grpcChannel = GrpcChannel.ForAddress(httpClient.BaseAddress, new GrpcChannelOptions { HttpClient = httpClient });
            this._grpcClient = Activator.CreateInstance(type: grpcClient, args: this._grpcChannel);
        }

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
