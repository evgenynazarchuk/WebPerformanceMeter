namespace WebPerformanceMeter.Logger.BrowserLog
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Collections.Concurrent;
    using System.IO;
    using System.Text.Json;
    using System.Threading;

    public class BrowserLogger : FileLogger
    {
        public BrowserLogger() { }
    }
}
