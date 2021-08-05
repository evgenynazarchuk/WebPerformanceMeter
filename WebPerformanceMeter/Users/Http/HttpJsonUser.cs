using System;
using System.Net.Http;
using WebPerformanceMeter.Tools.HttpTool;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;
using System.Text;

namespace WebPerformanceMeter.Users
{
    public abstract partial class HttpUser : User
    {
        /// <summary>
        /// Http Request for send and receive json objects
        /// </summary>
        /// <typeparam name="RequestObjectType"></typeparam>
        /// <typeparam name="ResponseObjectType"></typeparam>
        /// <param name="httpMethod"></param>
        /// <param name="requestUri"></param>
        /// <param name="requestObject"></param>
        /// <param name="requestHeaders"></param>
        /// <returns></returns>
        public Task<ResponseObjectType?> RequestAsJsonAsync<RequestObjectType, ResponseObjectType>(
            HttpMethod httpMethod,
            string requestUri,
            RequestObjectType requestObject,
            Dictionary<string, string>? requestHeaders = null)
            where RequestObjectType : class, new()
            where ResponseObjectType : class, new()
        {
            return Tool.RequestAsJsonAsync<RequestObjectType, ResponseObjectType>(
                httpMethod, 
                requestUri, 
                requestObject, 
                requestHeaders, 
                this.UserName);
        }

        /// <summary>
        /// Http Request for send json object
        /// </summary>
        /// <typeparam name="RequestObjectType"></typeparam>
        /// <param name="httpMethod"></param>
        /// <param name="requestUri"></param>
        /// <param name="requestObject"></param>
        /// <param name="requestHeaders"></param>
        /// <returns></returns>
        public Task<int> RequestAsJsonAsync<RequestObjectType>(
            HttpMethod httpMethod,
            string requestUri,
            RequestObjectType requestObject,
            Dictionary<string, string>? requestHeaders = null)
            where RequestObjectType : class, new()
        {

            return Tool.RequestAsJsonAsync<RequestObjectType>(
                httpMethod, 
                requestUri, 
                requestObject, 
                requestHeaders, 
                this.UserName);
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
