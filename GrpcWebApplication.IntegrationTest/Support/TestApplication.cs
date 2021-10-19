using Microsoft.AspNetCore.Mvc.Testing;

namespace GrpcWebApplication.IntegrationTest.Support
{
    public class TestApplication : WebApplicationFactory<Startup>
    {
        public TestApplication()
        {
        }
    }
}
