using System.Threading.Tasks;
using WebPerformanceMeter.DataReader.CsvReader;
using WebPerformanceMeter.PerformancePlans;
using WebPerformanceMeter.Scenario;

namespace Tests.Tests.GetPersonInfo
{
    public class TestPerformance
    {
        public async Task RunAsync()
        {
            var app = new WebApp();

            var csv = new CsvReader<Person>("Tests/GetPersonInfo/Person.csv");
            var user = new TestUser(app.Client);

            var performancePlan = new ConstantUsers(
                user: user,
                usersCount: 5,
                userLoopCount: 2,
                dataReader: csv,
                reuseDataInLoop: false);

            var scenario = new Scenario();

            scenario.AddPerformancePlan(performancePlan);
            await scenario.RunAsync();
        }
    }
}
