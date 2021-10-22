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
        Task<ResponseObjectType?> RequestAsJson<ResponseObjectType, RequestObjectType>(
            HttpMethod httpMethod,
            string requestUri,
            RequestObjectType requestObject,
            string requestLabel = "",
            Dictionary<string, string>? requestHeaders = null)
            where RequestObjectType : class, new()
            where ResponseObjectType : class, new();

        Task<string> RequestAsJson<RequestObjectType>(
            HttpMethod httpMethod,
            string requestUri,
            RequestObjectType requestObject,
            string requestLabel = "",
            Dictionary<string, string>? requestHeaders = null)
            where RequestObjectType : class, new();

        Task<ResponseObjectType?> RequestAsJson<ResponseObjectType>(
            HttpMethod httpMethod,
            string requestUri,
            string requestLabel = "",
            Dictionary<string, string>? requestHeaders = null)
            where ResponseObjectType : class, new();

        Task<ResponseObjectType?> GetAsJson<ResponseObjectType>(
            string path,
            string requestLabel = "",
            Dictionary<string, string>? requestHeaders = null)
            where ResponseObjectType : class, new();

        Task<string> PostAsJson<RequestObjectType>(
            string path,
            RequestObjectType requestObject,
            string requestLabel = "",
            Dictionary<string, string>? requestHeaders = null)
            where RequestObjectType : class, new();

        Task<string> PutAsJson<RequestObjectType>(
            string path,
            RequestObjectType requestObject,
            string requestLabel = "",
            Dictionary<string, string>? requestHeaders = null)
            where RequestObjectType : class, new();

        Task<ResponseObjectType?> PostAsJson<ResponseObjectType, RequestObjectType>(
            string path,
            RequestObjectType requestObject,
            string requestLabel = "",
            Dictionary<string, string>? requestHeaders = null)
            where ResponseObjectType : class, new()
            where RequestObjectType : class, new();

        Task<ResponseObjectType?> PutAsJson<ResponseObjectType, RequestObjectType>(
            string path,
            RequestObjectType requestObject,
            string requestLabel = "",
            Dictionary<string, string>? requestHeaders = null)
            where ResponseObjectType : class, new()
            where RequestObjectType : class, new();
    }
}
