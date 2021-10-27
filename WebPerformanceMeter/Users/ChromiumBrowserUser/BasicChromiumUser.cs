using System;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Reports;

namespace WebPerformanceMeter.Users
{
    public abstract class BasicChromiumUser : BasicUser, IDisposable
    {
        public BasicChromiumUser(string userName)
            : base(userName)
        {
            this.BrowserTool = new ChromiumTool(this.UserName, this.Watcher);
        }

        public void Dispose()
        {
            this.BrowserTool.DisposeAsync().GetAwaiter().GetResult();
        }

        protected readonly ChromiumTool BrowserTool;
    }
}
