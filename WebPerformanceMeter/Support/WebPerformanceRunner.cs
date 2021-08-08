using System;
using System.Collections.Generic;
using System.Reflection;
using WebPerformanceMeter.Attributes;
using System.Threading.Tasks;

namespace WebPerformanceMeter.Support
{
    public class WebPerformanceRunner
    {
        public static async Task ManualAsync(Assembly assembly)
        {
            // get test method
            Type[] assemblyTypes = assembly.GetTypes();
            Dictionary<int, (Type, MethodInfo)> testsList = new();

            int testNumber = 1;
            foreach (var assemblyType in assemblyTypes)
            {
                var assemblyTypeMethodsInfo = assemblyType.GetMethods();
                foreach (var methodInfo in assemblyTypeMethodsInfo)
                {
                    foreach (var attribute in methodInfo.CustomAttributes)
                    {
                        if (attribute.AttributeType == typeof(PerformanceTestAttribute))
                        {
                            testsList.Add(testNumber++, (assemblyType, methodInfo));
                        }
                    }
                }
            }

            // print welcome
            foreach (var testInfo in testsList)
            {
                Console.WriteLine($"{testInfo.Key} - {testInfo.Value.Item1.Name}.{testInfo.Value.Item2.Name}");
            }

            Console.Write($"Enter test number: ");
            if (!Int32.TryParse(Console.ReadLine(), out int selectedTestNumber))
            {
                throw new ApplicationException("Test number is incorrect");
            }

            // start performance test
            var testClass = Activator.CreateInstance(testsList[selectedTestNumber].Item1);
            if (testClass is null)
            {
                throw new ApplicationException("Error create test");
            }

            var testTask = testsList[selectedTestNumber].Item2.Invoke(testClass, null);
            if (testTask is not null)
            {
                await (Task)testTask;
            }
            else
            {
                throw new ApplicationException("Error run test");
            }
        }
    }
}
