﻿using System;
using System.Net.Http;
using WebPerformanceMeter.Tools.HttpTool;

namespace WebPerformanceMeter.Users
{
    public abstract class HttpUser : User
    {
        protected readonly HttpClient Client;

        protected readonly HttpTool Tool;

        public HttpUser(HttpClient client)
        {
            Client = client;
            Tool = new(Client);
        }

        public HttpUser(string host)
        {
            Client = new HttpClient()
            {
                BaseAddress = new Uri(host)
            };

            Tool = new(Client);
        }
    }
}
