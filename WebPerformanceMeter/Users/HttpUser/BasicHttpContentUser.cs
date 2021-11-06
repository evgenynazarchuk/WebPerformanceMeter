using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;

namespace WebPerformanceMeter.Users
{
    public abstract partial class BasicHttpUser : BasicUser
    {
        public Task<HttpResponse> GetAsync(
            string requestUri,
            Dictionary<string, string>? requestHeaders = null,
            string? requestContent = null,
            Encoding? contentEncoding = null)
        {
            return this.Tool.RequestAsync(
                httpMethod: HttpMethod.Get,
                path: requestUri,
                requestContent: new StringContent(requestContent ?? "", contentEncoding),
                requestHeaders: requestHeaders);
        }

        public Task<HttpResponse> PostAsync(
            string requestUri,
            Dictionary<string, string>? requestHeaders = null,
            string? requestContent = null,
            Encoding? contentEncoding = null)
        {
            return this.Tool.RequestAsync(
                httpMethod: HttpMethod.Post,
                path: requestUri,
                requestContent: new StringContent(requestContent ?? "", contentEncoding),
                requestHeaders: requestHeaders);
        }

        public Task<HttpResponse> PutAsync(
            string requestUri,
            Dictionary<string, string>? requestHeaders = null,
            string? requestContent = null,
            Encoding? contentEncoding = null)
        {
            return this.Tool.RequestAsync(
                httpMethod: HttpMethod.Put,
                path: requestUri,
                requestHeaders: requestHeaders,
                requestContent: new StringContent(requestContent ?? "", contentEncoding));
        }

        public Task<HttpResponse> DeleteAsync(
            string requestUri,
            Dictionary<string, string>? requestHeaders = null,
            string? requestContent = null,
            Encoding? contentEncoding = null)
        {
            return this.Tool.RequestAsync(
                httpMethod: HttpMethod.Delete,
                path: requestUri,
                requestHeaders: requestHeaders,
                requestContent: new StringContent(requestContent ?? "", contentEncoding));
        }
    }
}
