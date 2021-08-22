using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using Microsoft.Playwright;

namespace WebPerformanceMeter.Users
{
    public abstract class PerformanceUser
    {
        public string UserName { get; private set; }

        public PerformanceUser()
        {
            this.UserName = "";
        }

        protected void SetUserName(string userName)
        {
            this.UserName = userName;
        }

        public abstract Task InvokeAsync(int loopCount, IEntityReader? dataReader, bool reuseDataInLoop);
    }
}