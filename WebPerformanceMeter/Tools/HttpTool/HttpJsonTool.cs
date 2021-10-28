using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebPerformanceMeter
{
    public partial class HttpTool
    {
        private static readonly JsonSerializerOptions JsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        // request: send, receive
        public async Task<ResponseObjectType?> RequestAsJsonAsync<ResponseObjectType, RequestObjectType>(
            HttpMethod httpMethod,
            string requestUri,
            RequestObjectType requestObject,
            Dictionary<string, string>? requestHeaders = null,
            string userName = "",
            string requestLabel = "")
            where ResponseObjectType : class, new()
            where RequestObjectType : class, new()
        {
            string requestContent = JsonSerializer.Serialize(requestObject, JsonSerializerOptions);

            var response = await this.RequestAsync(
                httpMethod: httpMethod,
                path: requestUri,
                requestContent: new StringContent(requestContent, Encoding.UTF8, "application/json"),
                requestHeaders: requestHeaders,
                userName: userName,
                requestLabel: requestLabel);

            var responseObject = JsonSerializer.Deserialize<ResponseObjectType>(response.ContentAsBytes, JsonSerializerOptions);

            return responseObject;
        }

        // request: send
        public async Task<string> RequestAsJsonAsync<RequestObjectType>(
            HttpMethod httpMethod,
            string requestUri,
            RequestObjectType requestObject,
            Dictionary<string, string>? requestHeaders = null,
            string userName = "",
            string requestLabel = "")
            where RequestObjectType : class, new()
        {
            string requestContentString = JsonSerializer.Serialize(requestObject, JsonSerializerOptions);

            HttpResponse response = await this.RequestAsync(
                httpMethod: httpMethod,
                path: requestUri,
                requestContent: new StringContent(requestContentString, Encoding.UTF8, "application/json"),
                requestHeaders: requestHeaders,
                userName: userName,
                requestLabel: requestLabel);

            return response.ContentAsUtf8String;
        }

        // request: receive
        public async Task<ResponseObjectType?> RequestAsJsonAsync<ResponseObjectType>(
            HttpMethod httpMethod,
            string requestUri,
            Dictionary<string, string>? requestHeaders = null,
            string user = "",
            string requestLabel = "")
            where ResponseObjectType : class, new()
        {
            HttpResponse response = await this.RequestAsync(
                httpMethod: httpMethod,
                path: requestUri,
                requestHeaders: requestHeaders,
                userName: user,
                requestLabel: requestLabel);

            ResponseObjectType? responseObject = JsonSerializer.Deserialize<ResponseObjectType>(response.ContentAsUtf8String, JsonSerializerOptions);

            return responseObject;
        }
    }
}
