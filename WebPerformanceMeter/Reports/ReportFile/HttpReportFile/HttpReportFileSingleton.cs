namespace WebPerformanceMeter.Reports
{
    public class HttpReportFileSingleton
    {
        public static HttpReportFile GetInstance(string projectName, string testRunId)
        {
            if (_singleton is null)
            {
                lock (_lock)
                {
                    if (_singleton is null)
                    {
                        _singleton = new HttpReportFile(projectName, testRunId);
                    }
                }
            }

            return _singleton;
        }

        private HttpReportFileSingleton() { }

        private readonly static object _lock = new();

        private static HttpReportFile? _singleton = null;
    }
}
