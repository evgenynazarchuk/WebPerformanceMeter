using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WebPerformanceMeter.Tools.HttpTool;

namespace WebPerformanceMeter.Interfaces
{
    public interface IBaseHttpUser : IBaseUser
    {
        IHttpTool Tool { get; }

        Task<HttpResponse> RequestAsync(
            HttpMethod httpMethod,
            string path,
            Dictionary<string, string>? requestHeaders = null,
            HttpContent? requestContent = null,
            string userName = "",
            string requestLabel = "");
    }
}
