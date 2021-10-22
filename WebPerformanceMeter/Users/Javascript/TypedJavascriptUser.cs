using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Logger;
using WebPerformanceMeter.Users;

namespace WebPerformanceMeter
{
    public abstract partial class JavascriptUser<TData> : BasicJavascriptUser, ITypedUser<TData>
        where TData : class
    {
        public JavascriptUser(string userName = "", ILogger? logger = null)
            : base(userName, logger) { }

        public async Task InvokeAsync(
            IDataReader<TData> dataReader,
            bool reuseDataInLoop = true,
            int userloopCount = 1)
        {
            ////this.Page.RequestFinished += (_, request) =>
            ////{
            ////    Console.WriteLine($"{TimeSpan.FromMilliseconds(request.Timing.ConnectStart)} " +
            ////        $"{TimeSpan.FromMilliseconds(request.Timing.ConnectEnd)} " +
            ////        $"{TimeSpan.FromMilliseconds(request.Timing.RequestStart)} " +
            ////        $"{TimeSpan.FromMilliseconds(request.Timing.ResponseStart)} " +
            ////        $"{TimeSpan.FromMilliseconds(request.Timing.ResponseEnd)} " +
            ////        $"{request.Method} " +
            ////        $"{request.Url}");
            ////};

            TData? data = dataReader.GetData();

            for (int i = 0; i < userloopCount; i++)
            {
                if (data is null)
                {
                    continue;
                }

                await PerformanceAsync(data);

                if (!reuseDataInLoop)
                {
                    data = dataReader.GetData();
                }
            }
        }

        protected abstract Task PerformanceAsync(TData data);
    }
}
