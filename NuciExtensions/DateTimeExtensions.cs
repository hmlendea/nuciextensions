using System;

namespace NuciExtensions
{
    /// <summary>
    /// <see cref="DateTime"/> extensions.
    /// </summary>
    public static class DateTimeExtensions
    {
        static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static TimeSpan GetElapsedUnixTime(DateTime time)
        {
            DateTime timeInUtc = time.ToUniversalTime();

            if (timeInUtc < Jan1st1970)
            {
                throw new ArgumentOutOfRangeException(nameof(time));
            }

            TimeSpan timeSpan = timeInUtc - Jan1st1970;

            return timeSpan;
        }
    }
}
