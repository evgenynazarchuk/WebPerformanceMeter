﻿using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebPerformanceMeter.Interfaces
{
    public interface IHttpJsonTool : ITool
    {
        Task<ResponseObjectType?> RequestAsJsonAsync<ResponseObjectType, RequestObjectType>(
            HttpMethod httpMethod,
            string requestUri,
            RequestObjectType requestObject,
            Dictionary<string, string>? requestHeaders = null,
            string userName = "",
            string requestLabel = "")
            where ResponseObjectType : class, new()
            where RequestObjectType : class, new();


        Task<string> RequestAsJsonAsync<RequestObjectType>(
            HttpMethod httpMethod,
            string requestUri,
            RequestObjectType requestObject,
            Dictionary<string, string>? requestHeaders = null,
            string userName = "",
            string requestLabel = "")
            where RequestObjectType : class, new();

        Task<ResponseObjectType?> RequestAsJsonAsync<ResponseObjectType>(
            HttpMethod httpMethod,
            string requestUri,
            Dictionary<string, string>? requestHeaders = null,
            string user = "",
            string requestLabel = "")
            where ResponseObjectType : class, new();
    }
}
