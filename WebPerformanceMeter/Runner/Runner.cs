using System;
using System.Collections.Generic;
using System.Reflection;

namespace WebPerformanceMeter.Runner
{
    public class Runner
    {
        public static void Manual(Assembly assembly)
        {
            Type[] assemblyTypes = assembly.GetTypes();
            List<Type> testClassTypes = new();
            Dictionary<int, (Type, MethodInfo)> tests = new();

            foreach (var type in assemblyTypes)
            {
                foreach (var attr in type.CustomAttributes)
                {
                    if (attr.AttributeType == typeof(TestClassAttribute))
                    {
                        testClassTypes.Add(type);
                    }
                }
            }

            int testNumber = 1;
            foreach (var testClassType in testClassTypes)
            {
                var methods = testClassType.GetMethods();
                foreach (var method in methods)
                {
                    foreach (var attr in method.CustomAttributes)
                    {
                        if (attr.AttributeType == typeof(TestAttribute))
                        {
                            tests.Add(testNumber++, (testClassType, method));
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

            var testMethod = test.GetType().GetMethods();
            foreach (var method in testMethod)
            {
                if (method == tests[selectedTestNumber].Item2)
                {
                    method.Invoke(test, null);
                }
            }
        }
    }
}
