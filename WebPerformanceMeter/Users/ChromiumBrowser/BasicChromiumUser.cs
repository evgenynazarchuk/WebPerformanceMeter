using System;
using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Logger;

namespace WebPerformanceMeter.Users
{
    public abstract class BasicChromiumUser : BaseUser, IDisposable
    {
        protected readonly BrowserTool BrowserTool;

        public BasicChromiumUser(string userName = "", ILogger? logger = null)
            : base(userName, logger ?? new ChromiumLogger())
        {
            this.BrowserTool = new BrowserTool(this.UserName, logger);
        }

        public void Dispose()
        {
            this.BrowserTool.Dispose();
        }
    }
}
