using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using WebPerformanceMeter.Runner;

namespace WebPerformanceMeter
{
    public class WebRunner
    {
        public static void Start(Assembly assembly, WebServiceConfigDto config)
        {
            Host.CreateDefaultBuilder()
               .ConfigureWebHostDefaults(webBuilder =>
               {
                   webBuilder.UseStartup<TestRunnerStartup>();

                   webBuilder.ConfigureServices(services =>
                   {
                       services.AddSingleton<TestRunnerService>(x => new TestRunnerService(assembly));
                   });

                   webBuilder.UseUrls($"http://*:{config.TestRunnerPort}");
               })
               .Build()
               .Run();
        }
    }
}
