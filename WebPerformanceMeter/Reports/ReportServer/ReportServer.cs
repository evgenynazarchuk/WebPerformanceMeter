using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;

namespace WebPerformanceMeter.Reports
{
    public class ReportServer : Report
    {
        public ReportServer(string projectName, string testRunId)
            : base(projectName, testRunId) { }

        protected override Task ProcessAsync()
        {
            throw new NotImplementedException();
        }
    }
}
