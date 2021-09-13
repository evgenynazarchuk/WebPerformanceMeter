namespace GrpcWebApplication.IntegrationTest.Support
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.Testing;
    using GrpcWebApplication;

    public class TestApplication : WebApplicationFactory<Startup>
    {
        public TestApplication()
        { 
        }
    }
}
