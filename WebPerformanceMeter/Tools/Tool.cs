using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebPerformanceMeter.Logger;

namespace WebPerformanceMeter.Tools
{
    public class Tool
    {
        protected Watcher Watcher => Watcher.Instance.Value;
    }
}
