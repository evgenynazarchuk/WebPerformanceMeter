using System;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Report;

namespace WebPerformanceMeter.Users
{
    public abstract class BasicChromiumUser : BaseUser, IDisposable
    {
        protected readonly ChromiumTool BrowserTool;

        public BasicChromiumUser(string? userName = null, ILogger? logger = null)
            : base(userName ?? typeof(BasicChromiumUser).Name, logger ?? new ChromiumLogger())
        {
            this.BrowserTool = new ChromiumTool(this.UserName, logger);
        }

        public void Dispose()
        {
            this.BrowserTool.DisposeAsync().GetAwaiter().GetResult();
        }
    }
}
