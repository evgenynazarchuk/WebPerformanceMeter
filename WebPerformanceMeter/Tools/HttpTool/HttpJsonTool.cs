﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http;
using System.Net;

namespace WebPerformanceMeter.Tools.HttpTool
{
    public static class HttpJsonTool
    {
        private static readonly JsonSerializerOptions JsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        // request: send receive
        public static async Task<ResponseObjectType?> RequestAsJsonAsync<RequestObjectType, ResponseObjectType>(
            this HttpTool tool,
            HttpMethod httpMethod,
            string requestUri,
            RequestObjectType requestObject,
            Dictionary<string, string>? requestHeaders = null)
            where RequestObjectType : class, new()
            where ResponseObjectType : class, new()
        {
            string requestContent = JsonSerializer.Serialize(requestObject, JsonSerializerOptions);

            var response = await tool.RequestAsync(
                httpMethod: httpMethod,
                requestUri: requestUri,
                requestContent: new StringContent(requestContent, Encoding.UTF8, "application/json"),
                requestHeaders: requestHeaders);

            var responseObject = JsonSerializer.Deserialize<ResponseObjectType>(response.ContentAsBytes, JsonSerializerOptions);

            return responseObject;
        }

        // request: send
        public static async Task<HttpStatusCode> RequestAsJsonAsync<RequestObjectType>(
            this HttpTool tool,
            HttpMethod httpMethod,
            string requestUri,
            RequestObjectType requestObject,
            Dictionary<string, string>? requestHeaders = null
            )
            where RequestObjectType : class, new()
        {
            string requestContentString = JsonSerializer.Serialize(requestObject, JsonSerializerOptions);

            var response = await tool.RequestAsync(
                httpMethod: httpMethod,
                requestUri: requestUri,
                requestContent: new StringContent(requestContentString, Encoding.UTF8, "application/json"),
                requestHeaders: requestHeaders);

            return response.StatusCode;
        }

        // request: receive
        public static async Task<ResponseObjectType?> RequestAsJsonAsync<ResponseObjectType>(
            this HttpTool tool,
            HttpMethod httpMethod,
            string requestUri,
            Dictionary<string, string>? requestHeaders = null
            )
            where ResponseObjectType : class, new()
        {
            var response = await tool.RequestAsync(
                httpMethod: httpMethod,
                requestUri: requestUri,
                requestHeaders: requestHeaders);

            ResponseObjectType? responseObject = JsonSerializer.Deserialize<ResponseObjectType>(response.ContentAsUtf8String, JsonSerializerOptions);

            return responseObject;
        }
    }
}
