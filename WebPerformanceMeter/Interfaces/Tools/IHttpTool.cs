using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using WebPerformanceMeter.Tools.HttpTool;

namespace WebPerformanceMeter.Interfaces
{
    public interface IHttpTool : ITool, IHttpJsonTool
    {
        Task<HttpResponse> RequestAsync(
            HttpRequestMessage httpRequestMessage, 
            string userName = "", 
            string requestLabel = "");

        Task<HttpResponse> RequestAsync(
            HttpMethod httpMethod,
            string path,
            Dictionary<string, string>? requestHeaders = null,
            HttpContent? requestContent = null,
            string userName = "",
            string requestLabel = "");
    }
}
