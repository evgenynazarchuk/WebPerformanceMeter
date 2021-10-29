namespace WebPerformanceMeter.Reports
{
    public class GrpcReportFileSingleton
    {
        public static GrpcReportFile GetInstance(string projectName, string testRunId)
        {
            if (_singleton is null)
            {
                lock (_lock)
                {
                    if (_singleton is null)
                    {
                        _singleton = new GrpcReportFile(projectName, testRunId);
                    }
                }
            }

            return _singleton;
        }

        private GrpcReportFileSingleton() { }

        private readonly static object _lock = new ();

        private static GrpcReportFile? _singleton = null;
    }
}
