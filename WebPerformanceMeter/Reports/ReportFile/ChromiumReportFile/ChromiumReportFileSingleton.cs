namespace WebPerformanceMeter.Reports
{
    public class ChromiumReportFileSingleton
    {
        public static ChromiumReportFile GetInstance(string projectName, string testRunId)
        {
            if (_singleton is null)
            {
                lock (_lock)
                {
                    if (_singleton is null)
                    {
                        _singleton = new ChromiumReportFile(projectName, testRunId);
                    }
                }
            }

            return _singleton;
        }

        private ChromiumReportFileSingleton() { }

        private static object _lock = new object();

        private static ChromiumReportFile? _singleton = null;
    }
}
