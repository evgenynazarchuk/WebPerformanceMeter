using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPerformanceMeter.PerformancePlans
{
    public abstract class PerformancePlan
    {
        public abstract Task StartAsync();
    }
}
