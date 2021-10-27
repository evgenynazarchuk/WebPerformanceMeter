using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Users;

namespace WebPerformanceMeter
{
    public abstract partial class JavascriptUser : BasicJavascriptUser, ISimpleUser
    {
        public JavascriptUser(string? userName = null)
            : base(userName ?? typeof(JavascriptUser).Name) { }

        public async Task InvokeAsync(int loopCount = 1)
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

            for (int i = 0; i < loopCount; i++)
            {
                await PerformanceAsync();
            }
        }

        protected abstract Task PerformanceAsync();
    }
}
