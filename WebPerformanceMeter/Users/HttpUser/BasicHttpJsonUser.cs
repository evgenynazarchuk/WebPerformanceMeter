using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebPerformanceMeter.Users
{
    public abstract partial class BasicHttpUser : BasicUser
    {
        //
        public Task<ResponseObjectType?> RequestAsJson<ResponseObjectType, RequestObjectType>(
            HttpMethod httpMethod,
            string requestUri,
            RequestObjectType requestObject,
            string requestLabel = "",
            Dictionary<string, string>? requestHeaders = null)
            where RequestObjectType : class, new()
            where ResponseObjectType : class, new()
        {
            return this.Tool.RequestAsJsonAsync<ResponseObjectType, RequestObjectType>(
                httpMethod,
                requestUri,
                requestObject,
                requestHeaders,
                this.UserName,
                requestLabel);
        }

        public async Task<string> RequestAsJson<RequestObjectType>(
            HttpMethod httpMethod,
            string requestUri,
            RequestObjectType requestObject,
            string requestLabel = "",
            Dictionary<string, string>? requestHeaders = null)
            where RequestObjectType : class, new()
        {
            var httpResult = await this.Tool.RequestAsJsonAsync<RequestObjectType>(
                httpMethod,
                requestUri,
                requestObject,
                requestHeaders,
                this.UserName,
                requestLabel);

            return httpResult;
        }

        public Task<ResponseObjectType?> RequestAsJson<ResponseObjectType>(
            HttpMethod httpMethod,
            string requestUri,
            string requestLabel = "",
            Dictionary<string, string>? requestHeaders = null)
            where ResponseObjectType : class, new()
        {
            return this.Tool.RequestAsJsonAsync<ResponseObjectType>(
                httpMethod,
                requestUri,
                requestHeaders,
                this.UserName,
                requestLabel);
        }

        // string <-> object
        public Task<ResponseObjectType?> GetAsJson<ResponseObjectType>(
            string path,
            string requestLabel = "",
            Dictionary<string, string>? requestHeaders = null)
            where ResponseObjectType : class, new()
        {
            return this.Tool.RequestAsJsonAsync<ResponseObjectType>(
                HttpMethod.Get,
                path,
                requestHeaders,
                this.UserName,
                requestLabel);
        }

        public Task<string> PostAsJson<RequestObjectType>(
            string path,
            RequestObjectType requestObject,
            string requestLabel = "",
            Dictionary<string, string>? requestHeaders = null)
            where RequestObjectType : class, new()
        {
            return this.Tool.RequestAsJsonAsync<RequestObjectType>(
                HttpMethod.Post,
                path,
                requestObject,
                requestHeaders,
                this.UserName,
                requestLabel);
        }

        public Task<string> PutAsJson<RequestObjectType>(
            string path,
            RequestObjectType requestObject,
            string requestLabel = "",
            Dictionary<string, string>? requestHeaders = null)
            where RequestObjectType : class, new()
        {
            return this.Tool.RequestAsJsonAsync<RequestObjectType>(
                HttpMethod.Put,
                path,
                requestObject,
                requestHeaders,
                this.UserName,
                requestLabel);
        }

        public Task<string> DeleteAsJson<RequestObjectType>(
            string path,
            RequestObjectType requestObject,
            string requestLabel = "",
            Dictionary<string, string>? requestHeaders = null)
            where RequestObjectType : class, new()
        {
            return this.Tool.RequestAsJsonAsync<RequestObjectType>(
                HttpMethod.Delete,
                path,
                requestObject,
                requestHeaders,
                this.UserName,
                requestLabel);
        }

        // object <-> object
        public Task<ResponseObjectType?> PostAsJson<ResponseObjectType, RequestObjectType>(
            string path,
            RequestObjectType requestObject,
            string requestLabel = "",
            Dictionary<string, string>? requestHeaders = null)
            where ResponseObjectType : class, new()
            where RequestObjectType : class, new()
        {
            return this.Tool.RequestAsJsonAsync<ResponseObjectType, RequestObjectType>(
                HttpMethod.Post,
                path,
                requestObject,
                requestHeaders,
                this.UserName,
                requestLabel);
        }

        public Task<ResponseObjectType?> PutAsJson<ResponseObjectType, RequestObjectType>(
            string path,
            RequestObjectType requestObject,
            string requestLabel = "",
            Dictionary<string, string>? requestHeaders = null)
            where ResponseObjectType : class, new()
            where RequestObjectType : class, new()
        {
            return this.Tool.RequestAsJsonAsync<ResponseObjectType, RequestObjectType>(
                HttpMethod.Put,
                path,
                requestObject,
                requestHeaders,
                this.UserName,
                requestLabel);
        }

        public Task<ResponseObjectType?> DeleteAsJson<ResponseObjectType, RequestObjectType>(
            string path,
            RequestObjectType requestObject,
            string requestLabel = "",
            Dictionary<string, string>? requestHeaders = null)
            where ResponseObjectType : class, new()
            where RequestObjectType : class, new()
        {
            return this.Tool.RequestAsJsonAsync<ResponseObjectType, RequestObjectType>(
                HttpMethod.Delete,
                path,
                requestObject,
                requestHeaders,
                this.UserName,
                requestLabel);
        }
        //
    }
}
