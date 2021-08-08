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
            Type[] assemblyTypes = assembly.GetTypes();
            Dictionary<int, (Type, MethodInfo)> tests = new();

            int testNumber = 1;
            foreach (var type in assemblyTypes)
            {
                var methodsInfo = type.GetMethods();
                foreach (var methodInfo in methodsInfo)
                {
                    foreach (var attribute in methodInfo.CustomAttributes)
                    {
                        if (attribute.AttributeType == typeof(PerformanceTestAttribute))
                        {
                            tests.Add(testNumber++, (type, methodInfo));
                        }
                    }
                }
            }

            foreach (var item in tests)
            {
                Console.WriteLine($"{item.Key} {item.Value.Item1.Name} {item.Value.Item2.Name}");
            }

            Console.Write($"Enter test number: ");
            if (!Int32.TryParse(Console.ReadLine(), out int selectedTestNumber))
            {
                throw new ApplicationException("Test number is incorrect");
            }

            var test = Activator.CreateInstance(tests[selectedTestNumber].Item1);
            if (test is null)
            {
                throw new ApplicationException("Error test create");
            }

            var testMethodsInfo = test.GetType().GetMethods();
            foreach (var methodInfo in testMethodsInfo)
            {
                if (methodInfo == tests[selectedTestNumber].Item2)
                {
                    var task = methodInfo.Invoke(test, null);

                    if (task is not null)
                    {
                        await (Task)task;
                    }
                }
            }
        }
    }
}
