using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using System.Net;
using System.Xml;
using WebPerformanceMeter.Extensions;

namespace WebPerformanceMeter.Users
{
    public abstract partial class BasicHttpUser : BasicUser
    {
        // TODO сделать настройки из конфига
        private readonly XmlWriterSettings _xmlSerializationOptions = new()
        {
            Indent = true,
            OmitXmlDeclaration = true,
            CheckCharacters = false,
            Encoding = Encoding.UTF8
        };

        private static void AddXmlAcceptHeader(ref Dictionary<string, string>? headers)
        {
            if (headers is null)
            {
                headers = new();
            }

            headers.Add("Accept", "application/xml");
        }

        // get html
        public async Task<XmlDocument> GetXmlDocumentAsync(
            string requestUri,
            Dictionary<string, string>? requestHeaders = null
            )
        {
            HttpResponse response = await this.Tool.RequestAsync(
                httpMethod: HttpMethod.Get,
                path: requestUri,
                requestHeaders: requestHeaders);

            var content = response.ContentAsUtf8String;

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(content);

            return xmlDocument;
        }

        // xml input output
        public async Task<TypeResponseObject?> RequestXmlAsync<TypeResponseObject, TypeRequestObject>(
            HttpMethod httpMethod,
            string requestUri,
            TypeRequestObject requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where TypeRequestObject : class, new()
            where TypeResponseObject : class, new()
        {
            // TODO сделать чтение параметра Accept из конфига
            // так как могут быть application/xml или text/xml
            BasicHttpUser.AddXmlAcceptHeader(ref requestHeaders);

            string requestXmlContentString = requestObject.FromObjectToXmlString(this._xmlSerializationOptions);

            HttpResponse response = await this.Tool.RequestAsync(
                httpMethod: httpMethod,
                path: requestUri,
                requestContent: new StringContent(requestXmlContentString, requestContentEncoding, mediaType: "application/xml"),
                requestHeaders: requestHeaders);

            TypeResponseObject? responseObject = response.ContentAsUtf8String.FromXmlStringToObject<TypeResponseObject>();

            return responseObject;
        }

        // xml input
        public async Task<HttpStatusCode> RequestXmlAsync<TypeRequestObject>(
            HttpMethod httpMethod,
            string requestUri,
            TypeRequestObject requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where TypeRequestObject : class, new()
        {
            string requestXmlContentString = requestObject.FromObjectToXmlString(this._xmlSerializationOptions);

            HttpResponse response = await this.Tool.RequestAsync(
                httpMethod: httpMethod,
                path: requestUri,
                requestContent: new StringContent(requestXmlContentString, requestContentEncoding, "application/xml"),
                requestHeaders: requestHeaders);

            return (HttpStatusCode)response.StatusCode;
        }

        // xml output
        public async Task<TypeResponseObject?> RequestXmlAsync<TypeResponseObject>(
            HttpMethod httpMethod,
            string requestUri,
            Dictionary<string, string>? requestHeaders = null)
            where TypeResponseObject : class, new()
        {
            // TODO сделать чтение параметра Accept из конфига
            // так как могут быть application/xml или text/xml
            BasicHttpUser.AddXmlAcceptHeader(ref requestHeaders);

            HttpResponse response = await this.Tool.RequestAsync(
                httpMethod: httpMethod,
                path: requestUri,
                requestHeaders: requestHeaders);

            TypeResponseObject? responseObject = response.ContentAsUtf8String.FromXmlStringToObject<TypeResponseObject>();

            return responseObject;
        }

        // get output
        public Task<TypeResponseObject?> GetXmlAsync<TypeResponseObject>(
            string requestUri,
            Dictionary<string, string>? requestHeaders = null)
            where TypeResponseObject : class, new()
        {
            return this.RequestXmlAsync<TypeResponseObject>(
                httpMethod: HttpMethod.Get,
                requestUri: requestUri,
                requestHeaders: requestHeaders);
        }

        // get input
        public Task<HttpStatusCode> GetXmlAsync<TypeRequestObject>(
            string requestUri,
            TypeRequestObject requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where TypeRequestObject : class, new()
        {
            return this.RequestXmlAsync<TypeRequestObject>(
                httpMethod: HttpMethod.Get,
                requestUri: requestUri,
                requestObject: requestObject,
                requestHeaders: requestHeaders,
                requestContentEncoding: requestContentEncoding);
        }

        // get input output
        public Task<TypeResponseObject?> GetXmlAsync<TypeResponseObject, TypeRequestObject>(
            string requestUri,
            TypeRequestObject requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where TypeRequestObject : class, new()
            where TypeResponseObject : class, new()
        {
            return this.RequestXmlAsync<TypeResponseObject, TypeRequestObject>(
                httpMethod: HttpMethod.Get,
                requestUri: requestUri,
                requestObject: requestObject,
                requestHeaders: requestHeaders,
                requestContentEncoding: requestContentEncoding);
        }

        // post: input
        public Task<HttpStatusCode> PostXmlAsync<TypeRequestObject>(
            string requestUri,
            TypeRequestObject requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where TypeRequestObject : class, new()
        {
            return this.RequestXmlAsync<TypeRequestObject>(
                httpMethod: HttpMethod.Post,
                requestUri: requestUri,
                requestObject: requestObject,
                requestHeaders: requestHeaders,
                requestContentEncoding: requestContentEncoding);
        }

        // post: output
        public Task<TypeResponseObject?> PostXmlAsync<TypeResponseObject>(
            string requestUri,
            Dictionary<string, string>? requestHeaders = null
            )
            where TypeResponseObject : class, new()
        {
            return this.RequestXmlAsync<TypeResponseObject>(
                httpMethod: HttpMethod.Post,
                requestUri: requestUri,
                requestHeaders: requestHeaders);
        }

        // post: input output
        public Task<TypeResponseObject?> PostXmlAsync<TypeResponseObject, TypeRequestObject>(
            string requestUri,
            TypeRequestObject requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where TypeRequestObject : class, new()
            where TypeResponseObject : class, new()
        {
            return this.RequestXmlAsync<TypeResponseObject, TypeRequestObject>(
                httpMethod: HttpMethod.Post,
                requestUri: requestUri,
                requestObject: requestObject,
                requestHeaders: requestHeaders,
                requestContentEncoding: requestContentEncoding);
        }

        // put: output
        public Task<TypeResponseObject?> PutXmlAsync<TypeResponseObject>(
            string requestUri,
            Dictionary<string, string>? requestHeaders = null
            )
            where TypeResponseObject : class, new()
        {
            return this.RequestXmlAsync<TypeResponseObject>(
                httpMethod: HttpMethod.Put,
                requestUri: requestUri,
                requestHeaders: requestHeaders);
        }

        // put: input
        public Task<HttpStatusCode> PutXmlAsync<TypeRequestObject>(
            string requestUri,
            TypeRequestObject requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where TypeRequestObject : class, new()
        {
            return this.RequestXmlAsync<TypeRequestObject>(
                httpMethod: HttpMethod.Put,
                requestUri: requestUri,
                requestObject: requestObject,
                requestHeaders: requestHeaders,
                requestContentEncoding: requestContentEncoding);
        }

        // put: input output
        public Task<TypeResponseObject?> PutXmlAsync<TypeResponseObject, TypeRequestObject>(
            string requestUri,
            TypeRequestObject requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where TypeRequestObject : class, new()
            where TypeResponseObject : class, new()
        {
            return this.RequestXmlAsync<TypeResponseObject, TypeRequestObject>(
                httpMethod: HttpMethod.Put,
                requestUri: requestUri,
                requestObject: requestObject,
                requestHeaders: requestHeaders,
                requestContentEncoding: requestContentEncoding);
        }

        // delete: output
        public Task<TypeResponseObject?> DeleteXmlAsync<TypeResponseObject>(
            string requestUri,
            Dictionary<string, string>? requestHeaders = null
            )
            where TypeResponseObject : class, new()
        {
            return this.RequestXmlAsync<TypeResponseObject>(
                httpMethod: HttpMethod.Delete,
                requestUri: requestUri,
                requestHeaders: requestHeaders);
        }

        // delete: input
        public Task<HttpStatusCode> DeleteXmlAsync<TypeRequestObject>(
            string requestUri,
            TypeRequestObject requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where TypeRequestObject : class, new()
        {
            return this.RequestXmlAsync<TypeRequestObject>(
                httpMethod: HttpMethod.Delete,
                requestUri: requestUri, requestObject,
                requestHeaders: requestHeaders,
                requestContentEncoding: requestContentEncoding);
        }

        // delete: input output
        public Task<TypeResponseObject?> DeleteXmlAsync<TypeResponseObject, TypeRequestObject>(
            string requestUri,
            TypeRequestObject requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where TypeRequestObject : class, new()
            where TypeResponseObject : class, new()
        {
            return this.RequestXmlAsync<TypeResponseObject, TypeRequestObject>(
                httpMethod: HttpMethod.Delete,
                requestUri: requestUri,
                requestObject: requestObject,
                requestHeaders: requestHeaders,
                requestContentEncoding: requestContentEncoding);
        }
    }
}
