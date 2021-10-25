using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using WebPerformanceMeter.Attributes;

namespace WebPerformanceMeter.TestRunnerWebService
{
    public class TestRunnerService
    {
        public TestRunnerService(Assembly assembly)
        {
            this._tests = new();

            var testAssemblyTypes = assembly.GetTypes()
                .Where(x => x.GetCustomAttributes()
                .Any(x => x is PerformanceClassAttribute))
                .ToList();

            foreach (var assemblyType in testAssemblyTypes)
            {
                foreach (var methodInfo in assemblyType.GetMethods())
                {
                    foreach (var attributes in methodInfo.GetCustomAttributes())
                    {
                        if (attributes is PerformanceTestAttribute)
                        {
                            this._tests.Add(assemblyType, methodInfo);
                            break;
                        }
                    }
                }
            }
        }

        public IEnumerable<TestMethodIdentityDto> GetTestsIdentifiers()
        {
            return this._tests.Select(x => new TestMethodIdentityDto
            {
                TestClassName = x.Key.Name,
                TestMethodName = x.Value.Name
            });
        }

        public TestMethodDetailsDto GetTestDetail(string testClassName, string testMethodName)
        {
            var testMethod = this._tests.FirstOrDefault(x => x.Key.Name == testClassName && x.Value.Name == testMethodName).Value;
            var testParameters = testMethod.GetParameters();
            var testParametersName = testParameters.Select(x => x.Name).ToList();
            var testAttributes = testMethod
                        .GetCustomAttributes()
                        .Where(x => x is PerformanceTestAttribute)
                        .Select(x => x as PerformanceTestAttribute);

            var testParametersValues = new List<object[]>();

            foreach (var testAttribute in testAttributes)
            {
                if (testAttribute is not null && testAttribute.Parameters is not null)
                {
                    testParametersValues.Add(testAttribute.Parameters);
                }
            }

            var testDetails = new TestMethodDetailsDto
            {
                TestClassName = testClassName,
                TestMethodName = testMethodName,
                ParametersNames = testParametersName,
                ParametersValues = testParametersValues
            };

            return testDetails;
        }

        public List<TestMethodDetailsDto> GetTestsDetails()
        {
            var testsDetails = new List<TestMethodDetailsDto>();
            var tests = this.GetTestsIdentifiers();

            foreach (var test in tests)
            {
                testsDetails.Add(this.GetTestDetail(test.TestClassName, test.TestMethodName));
            }

            return testsDetails;
        }

        public (Type, MethodInfo) GetTestMethod(string testClassName, string testMethodName)
        {
            var test = this._tests.FirstOrDefault(x => x.Key.Name == testClassName && x.Value.Name == testMethodName);
            return (test.Key, test.Value);
        }

        public Task StartTestAsync(StartTestMethodDto startTestDto)
        {
            if (this._status is not null)
            {
                throw new ApplicationException("Test Runner is busy");
            }

            var (testClassType, testMethodInfo) = this.GetTestMethod(startTestDto.TestClassName, startTestDto.TestMethodName);
            var testClass = testClassType.GetConstructors().First().Invoke(null);
            var parametersInfo = testMethodInfo.GetParameters();
            var parametersValues = new List<object>();

            if (parametersInfo.Count() != startTestDto.ParametersValues?.Count())
            {
                throw new ApplicationException("Parameters does not match");
            }

            // ad hoc
            // JsonElement object not convertable to integer, string, ....
            for (int i = 0; i < parametersInfo.Count(); i++)
            {
                var raw = ((JsonElement)startTestDto.ParametersValues[i]).GetRawText();
                parametersValues.Add(Convert.ChangeType(raw, parametersInfo[i].ParameterType));
            }

            this._status = new TestRunnertStatusDto
            {
                TestRunIdentifier = startTestDto.TestRunIdentifier,
                TestClassName = startTestDto.TestClassName,
                TestMethodName = startTestDto.TestMethodName,
                ParametersNames = parametersInfo.Select(x => x.Name).ToList(),
                ParametersValues = parametersValues.ToArray(),
                StartTime = DateTime.UtcNow
            };

            Task.Run(async () =>
            {
                var performanceTestTask = (Task?)testMethodInfo.Invoke(testClass, parametersValues.ToArray());

                if (performanceTestTask is not null)
                {
                    await performanceTestTask;
                }

                this._status = null;
            });

            return Task.CompletedTask;
        }

        public TestRunnertStatusDto? GetStatus()
        {
            return this._status;
        }

        private readonly Dictionary<Type, MethodInfo> _tests;

        private TestRunnertStatusDto? _status = null;
    }
}
