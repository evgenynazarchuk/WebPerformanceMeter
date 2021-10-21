using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace WebPerformanceMeter.Interfaces
{
    public interface IHttpJsonTool : ITool
    {
        Task<ResponseObjectType?> RequestAsJsonAsync<RequestObjectType, ResponseObjectType>(
            HttpMethod httpMethod,
            string requestUri,
            RequestObjectType requestObject,
            Dictionary<string, string>? requestHeaders = null,
            string userName = "",
            string requestLabel = "")
            where RequestObjectType : class, new()
            where ResponseObjectType : class, new();

        Task<int> RequestAsJsonAsync<RequestObjectType>(
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
