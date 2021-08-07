using System;
using System.Net.Http;
using WebPerformanceMeter.Tools.HttpTool;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;
using System.Text;

namespace WebPerformanceMeter.Users
{
    public partial class HttpUser : User
    {
        public Task<ResponseObjectType?> RequestAsJsonAsync<RequestObjectType, ResponseObjectType>(
            HttpMethod httpMethod,
            string requestUri,
            RequestObjectType requestObject,
            string requestLabel = "",
            Dictionary<string, string>? requestHeaders = null)
            where RequestObjectType : class, new()
            where ResponseObjectType : class, new()
        {
            return Tool.RequestAsJsonAsync<RequestObjectType, ResponseObjectType>(
                httpMethod, 
                requestUri, 
                requestObject, 
                requestHeaders, 
                this.UserName,
                requestLabel);
        }

        public Task<int> RequestAsJsonAsync<RequestObjectType>(
            HttpMethod httpMethod,
            string requestUri,
            RequestObjectType requestObject,
            string requestLabel = "",
            Dictionary<string, string>? requestHeaders = null)
            where RequestObjectType : class, new()
        {

            return Tool.RequestAsJsonAsync<RequestObjectType>(
                httpMethod, 
                requestUri, 
                requestObject, 
                requestHeaders, 
                this.UserName,
                requestLabel);
        }

        /// <summary>
        /// Http Request for receive json object
        /// </summary>
        /// <typeparam name="ResponseObjectType"></typeparam>
        /// <param name="httpMethod"></param>
        /// <param name="requestUri"></param>
        /// <param name="requestHeaders"></param>
        /// <returns></returns>
        public Task<ResponseObjectType?> RequestAsJsonAsync<ResponseObjectType>(
            HttpMethod httpMethod,
            string requestUri,
            string requestLabel = "",
            Dictionary<string, string>? requestHeaders = null
            )
            where ResponseObjectType : class, new()
        {
            return Tool.RequestAsJsonAsync<ResponseObjectType>(
                httpMethod, 
                requestUri, 
                requestHeaders, 
                this.UserName);
        }
    }
}
