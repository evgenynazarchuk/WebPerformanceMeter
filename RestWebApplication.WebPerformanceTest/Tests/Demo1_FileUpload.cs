using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebPerformanceMeter.Attributes;
using WebPerformanceMeter.Support;
using WebPerformanceMeter.PerformancePlans;
using RestWebApplication.WebPerformanceTest.Users;
using Microsoft.AspNetCore.Mvc.Testing;
using RestWebApplication;
using WebPerformanceMeter;
using WebPerformanceMeter.Extensions;

namespace RestWebApplication.WebPerformanceTest.Tests
{
    [PerformanceClass]
    public class Demo1_FileUpload
    {
        [PerformanceTest(10, 10)]
        [PerformanceTest(10, 60)]
        public async Task Test(int usersCount, int seconds)
        {
            var app = new WebApplicationFactory<Startup>();
            var httpClient = app.CreateClient();
            var user = new UploadFileUser(httpClient);
            var plan = new ActiveUsersOnPeriod(user, usersCount, seconds.Seconds());

            await new Scenario().AddSequentialPlans(plan).Start();
        }
    }
}
