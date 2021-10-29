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

        private readonly static object _lock = new ();

        private static ChromiumReportFile? _singleton = null;
    }
}
