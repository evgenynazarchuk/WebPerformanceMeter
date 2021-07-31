using System;
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
        public static async Task<TypeResponseObject?> RequestAsJsonAsync<TypeResponseObject, TypeRequestObject>(
            this HttpTool tool,
            HttpMethod httpMethod,
            string requestUri,
            TypeRequestObject? requestObject = null,
            Dictionary<string, string>? requestHeaders = null)
            where TypeRequestObject : class, new()
            where TypeResponseObject : class, new()
        {
            string requestContent = JsonSerializer.Serialize(requestObject, JsonSerializerOptions);

            var response = await tool.RequestAsync(
                httpMethod: httpMethod,
                requestUri: requestUri,
                requestContent: new StringContent(requestContent, Encoding.UTF8, "application/json"),
                requestHeaders: requestHeaders);

            var responseObject = JsonSerializer.Deserialize<TypeResponseObject>(response.ContentAsBytes, JsonSerializerOptions);

            return responseObject;
        }

        // request: send
        public static async Task<HttpStatusCode> RequestAsJsonAsync<TypeRequestObject>(
            this HttpTool tool,
            HttpMethod httpMethod,
            string requestUri,
            TypeRequestObject requestObject,
            Dictionary<string, string>? requestHeaders = null
            )
            where TypeRequestObject : class, new()
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
        public static async Task<TypeResponseObject?> RequestAsJsonAsync<TypeResponseObject>(
            this HttpTool tool,
            HttpMethod httpMethod,
            string requestUri,
            Dictionary<string, string>? requestHeaders = null
            )
            where TypeResponseObject : class, new()
        {
            var response = await tool.RequestAsync(
                httpMethod: httpMethod,
                requestUri: requestUri,
                requestHeaders: requestHeaders);

            TypeResponseObject? responseObject = JsonSerializer.Deserialize<TypeResponseObject>(response.ContentAsUtf8String, JsonSerializerOptions);

            return responseObject;
        }
    }
}
