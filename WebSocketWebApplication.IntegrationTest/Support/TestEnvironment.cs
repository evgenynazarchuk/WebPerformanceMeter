using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.WebSockets;
using Microsoft.AspNetCore.TestHost;

namespace WebSocketWebApplication.IntegrationTest.Support
{
    internal class TestEnvironment
    {
        public readonly TestApplication App;

        public TestEnvironment()
        { 
            this.App = new TestApplication();
        }
    }
}
