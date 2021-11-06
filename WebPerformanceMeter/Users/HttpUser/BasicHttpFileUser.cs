using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using System.Net;
using System.Net.Http.Headers;
using System;

namespace WebPerformanceMeter.Users
{
    public abstract partial class BasicHttpUser : BasicUser
    {
        public async Task<HttpStatusCode> UploadFile(
            string requestUri,
            string fileName,
            byte[] file,
            Dictionary<string, string>? requestHeaders = null,
            string httpFileParameter = "file")
        {
            using var form = new MultipartFormDataContent();
            using var fileContent = new ByteArrayContent(file);

            // TODO сделать автоопределение ContentType
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");
            form.Add(fileContent, httpFileParameter, fileName);

            var response = await this.Tool.RequestAsync(
                httpMethod: HttpMethod.Post,
                path: requestUri,
                requestHeaders: requestHeaders,
                requestContent: form);

            return (HttpStatusCode)response.StatusCode;
        }

        public async Task<HttpStatusCode> UploadFiles(
            string requestUri,
            IEnumerable<(string, byte[])> files,
            Dictionary<string, string>? requestHeaders = null,
            string httpFileParameter = "files")
        {
            var form = new MultipartFormDataContent();

            foreach ((string fileName, byte[] fileBytes) in files)
            {
                var fileContent = new ByteArrayContent(fileBytes);
                fileContent.Headers.ContentDisposition = new("form-data");
                fileContent.Headers.ContentDisposition.Name = httpFileParameter;
                fileContent.Headers.ContentDisposition.FileName = fileName;

                // TODO сделать автоопределение ContentType
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                form.Add(fileContent);
            }

            HttpResponse response = await this.Tool.RequestAsync(
                httpMethod: HttpMethod.Post,
                path: requestUri,
                requestHeaders: requestHeaders,
                requestContent: form);

            return (HttpStatusCode)response.StatusCode;
        }

        public async Task<(string?, byte[])> DownloadFile(string path)
        {
            HttpResponse response = await this.Tool.RequestAsync(HttpMethod.Get, path);
            string? fileName = response.Filename;
            byte[] bytes = response.ContentAsBytes;

            return (fileName, bytes);
        }
    }
}
