using System.Threading.Tasks;

namespace WebPerformanceMeter.Extensions
{
    public static class TaskExtension
    {
        public static async Task Wait(this Task[,] tasks, int nLength, int mLength)
        {
            for (var i = 0; i < nLength; i++)
            {
                for (var j = 0; j < mLength; j++)
                {
                    if (tasks[i, j] is not null)
                    {
                        await tasks[i, j];
                    }
                }
            }
        }
    }
}
