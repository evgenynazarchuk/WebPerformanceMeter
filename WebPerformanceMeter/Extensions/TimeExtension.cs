namespace WebPerformanceMeter.Extensions
{
    using System;

    public static class TimeExtension
    {
        public static TimeSpan Seconds(this int duration) => TimeSpan.FromSeconds(duration);

        public static TimeSpan Minutes(this int duration) => TimeSpan.FromMinutes(duration);

        public static TimeSpan Hours(this int duration) => TimeSpan.FromHours(duration);

        public static TimeSpan Milliseconds(this int duration) => TimeSpan.FromMilliseconds(duration);
    }
}
