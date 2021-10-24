using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace WebPerformanceMeter.TestRunnerWebService
{
    public class WebServiceRunner
    {
        public static void Start(Assembly assembly)
        {
            Host.CreateDefaultBuilder()
               .ConfigureWebHostDefaults(webBuilder =>
               {
                   webBuilder.UseStartup<Startup>();

                   webBuilder.ConfigureServices(services =>
                   {
                       services.AddSingleton<TestRunner>(x => new TestRunner(assembly));
                   });
               })
               .Build()
               .Run();
        }
    }
}
