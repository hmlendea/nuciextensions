using System;

namespace NuciExtensions
{
    /// <summary>
    /// Provides extension methods for converting between <see cref="DateTime"/> and UNIX timestamps.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// The UNIX epoch start date, January 1st, 1970, at midnight UTC.
        /// </summary>
        static readonly DateTime Jan1st1970 = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Converts a <see cref="DateTime"/> to a UNIX timestamp.
        /// </summary>
        /// <param name="time">The date and time to convert.</param>
        /// <returns>The UNIX timestamp as a double.</returns>
        public static TimeSpan GetElapsedUnixTime(DateTime time)
        {
            DateTime timeInUtc = new(
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

            return timeInUtc - Jan1st1970;
        }

        /// <summary>
        /// Converts a UNIX timestamp to a <see cref="DateTime"/>.
        /// </summary>
        /// <param name="unixTimestamp">The UNIX timestamp as a string.</param>
        /// <returns>The corresponding <see cref="DateTime"/> in UTC.</returns>
        /// <exception cref="ArgumentException">Thrown if the string is not a valid UNIX timestamp.</exception>
        public static DateTime FromUnixTime(string unixTimestamp)
        {
            if (!double.TryParse(unixTimestamp, out double unixTime))
            {
                throw new ArgumentException("The specified string is not a valid UNIX timestamp");
            }

            return FromUnixTime(unixTime);
        }

        /// <summary>
        /// Converts a UNIX timestamp to a <see cref="DateTime"/>.
        /// </summary>
        /// <param name="unixTime">The UNIX timestamp as a double.</param>
        /// <returns>The corresponding <see cref="DateTime"/> in UTC.</returns>
        public static DateTime FromUnixTime(double unixTime) => Jan1st1970.AddSeconds(unixTime);
    }
}
