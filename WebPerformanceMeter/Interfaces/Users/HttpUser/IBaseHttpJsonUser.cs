using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace WebPerformanceMeter.Interfaces
{
    public interface IBaseHttpJsonUser
    {
        Task<ResponseObjectType?> RequestAsJsonAsync<RequestObjectType, ResponseObjectType>(
            HttpMethod httpMethod,
            string requestUri,
            RequestObjectType requestObject,
            string requestLabel = "",
            Dictionary<string, string>? requestHeaders = null)
            where RequestObjectType : class, new()
            where ResponseObjectType : class, new();

        Task<int> RequestAsJsonAsync<RequestObjectType>(
            HttpMethod httpMethod,
            string requestUri,
            RequestObjectType requestObject,
            string requestLabel = "",
            Dictionary<string, string>? requestHeaders = null)
            where RequestObjectType : class, new();

        Task<ResponseObjectType?> RequestAsJsonAsync<ResponseObjectType>(
            HttpMethod httpMethod,
            string requestUri,
            string requestLabel = "",
            Dictionary<string, string>? requestHeaders = null)
            where ResponseObjectType : class, new();
    }
}
