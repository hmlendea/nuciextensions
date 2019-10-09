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
            DateTime timeInUtc = new DateTime(
                time.Year,
                time.Month,
                time.Day,
                time.Hour,
                time.Minute,
                time.Second,
                time.Millisecond,
                DateTimeKind.Utc);

            if (timeInUtc < Jan1st1970)
            {
                throw new ArgumentOutOfRangeException(nameof(time));
            }

            TimeSpan timeSpan = timeInUtc - Jan1st1970;

            return timeSpan;
        }

        public static DateTime FromUnixTime(string unixTimestamp)
        {
            double unixTime;
            bool isTimestampValid = double.TryParse(unixTimestamp, out unixTime);

            if (!isTimestampValid)
            {
                throw new ArgumentException("The specified string is not a valid UNIX timestamp");
            }

            return FromUnixTime(unixTime);
        }

        public static DateTime FromUnixTime(double unixTime)
        {
            DateTime dateTime = Jan1st1970.AddSeconds(unixTime);

            return dateTime;
        }
    }
}
