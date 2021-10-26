using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System.Reflection;
using WebPerformanceMeter.Runner;

namespace WebPerformanceMeter
{
    public class TestRunner
    {
        private string[] _args;

        private Assembly _assembly;

        public TestRunner(string[] args, Assembly assembly)
        {
            this._args = args;
            this._assembly = assembly;
        }

        public async Task StartAsync()
        {
            if (this._args.Count() == 0)
            {
                var runner = new ConsoleRunner(this._assembly);
                await runner.StartAsync();
            }
            else
            {
                int port = 0;
                string? loggerAddress = null;

                for (int i = 0; i < this._args.Count(); i++)
                {
                    if (this._args[i] == "-p")
                    {
                        port = int.Parse(this._args[i + 1]);
                        break;
                    }
                }

                for (int i = 0; i < this._args.Count(); i++)
                {
                    if (this._args[i] == "-l")
                    {
                        loggerAddress = this._args[i + 1];
                        break;
                    }
                }

                var config = new WebServiceConfigDto
                {
                    TestRunnerPort = port,
                    LogServiceAddress = loggerAddress
                };

                WebRunner.Start(this._assembly, config);
            }
        }
    }
}
