namespace GrpcWebApplication.IntegrationTest.Support
{
    using GrpcWebApplication;
    using Microsoft.AspNetCore.Mvc.Testing;

    public class TestApplication : WebApplicationFactory<Startup>
    {
        public TestApplication()
        {
        }
    }
}
