using System;

namespace NuciExtensions
{
    /// <summary>
    /// <see cref="DateTime"/> extensions.
    /// </summary>
    public static class DateTimeExtensions
    {
        static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long GetCurrentTimeMilliseconds(this DateTime source)
        {
            TimeSpan timeSpan = DateTime.UtcNow - Jan1st1970;
            return (long)timeSpan.TotalMilliseconds;
        }
    }
}
