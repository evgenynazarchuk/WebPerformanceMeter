namespace WebPerformanceMeter.Users
{
    using System;
    using System.Net.Http;
    using WebPerformanceMeter.Tools.HttpTool;

    public abstract partial class HttpUser : User
    {
        protected readonly HttpClient Client;

        protected readonly HttpTool Tool;

        public HttpUser(HttpClient client, string userName = "")
        {
            this.Client = client;
            this.Tool = new(Client);
            this.SetUserName(userName);
        }

        public HttpUser(string host)
        {
            this.Client = new HttpClient()
            {
                BaseAddress = new Uri(host)
            };

            this.Tool = new (this.Client);
        }
    }
}
