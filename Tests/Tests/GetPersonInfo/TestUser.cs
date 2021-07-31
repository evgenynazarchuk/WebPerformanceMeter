using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebPerformanceMeter;
using WebPerformanceMeter.Users;
using System.Net.Http;
using System.Text.Json;
using WebPerformanceMeter.Tools.HttpTool;

namespace Tests.Tests.GetPersonInfo
{
    public class TestUser : HttpUser
    {
        public TestUser(HttpClient client)
            : base(client) { }

        public override async Task PerformanceAsync(object entity)
        {
            JsonSerializerOptions option = new()
            {
                PropertyNameCaseInsensitive = false
            };

            var person = entity as Person;
            var content = new StringContent(JsonSerializer.Serialize(person, option), Encoding.UTF8, "application/json");

            

            var response = await Client.PostAsync("/Test/TestPersonMethod", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"response: {responseContent}");
        }
    }
}
