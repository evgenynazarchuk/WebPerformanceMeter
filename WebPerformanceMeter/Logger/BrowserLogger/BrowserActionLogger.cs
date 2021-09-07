namespace WebPerformanceMeter.Logger.BrowserLogger
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class BrowserActionLogger
    {
        public readonly ConcurrentQueue<string> BrowserActionLogQueue;
    }
}
