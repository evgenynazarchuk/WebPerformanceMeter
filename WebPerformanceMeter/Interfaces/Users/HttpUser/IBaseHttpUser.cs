using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebPerformanceMeter.Interfaces
{
    public interface IBaseHttpUser : IBaseUser
    {
        IHttpTool Tool { get; }

        Task<HttpResponse> Request(
            HttpMethod httpMethod,
            string path,
            Dictionary<string, string>? requestHeaders = null,
            HttpContent? requestContent = null,
            string requestLabel = "");

        Task<string> Get(
            string path,
            Dictionary<string, string>? requestHeaders = null,
            string requestLabel = "");
    }
}
