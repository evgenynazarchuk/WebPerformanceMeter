using System;

namespace WebPerformanceMeter.Reports
{
    public class WebSocketReportFileSingleton
    {
        public static WebSocketReportFile GetInstance(string projectName, string testRunId)
        {
            if (_singleton is null)
            {
                lock (_lock)
                {
                    if (_singleton is null)
                    {
                        _singleton = new WebSocketReportFile(projectName, testRunId);
                    }
                }
            }

            return _singleton;
        }

        private WebSocketReportFileSingleton() { }

        private static object _lock = new object();

        private static WebSocketReportFile? _singleton = null;
    }
}
