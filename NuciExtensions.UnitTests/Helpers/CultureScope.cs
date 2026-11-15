using System;
using System.Globalization;
using System.Threading;

namespace NuciExtensions.UnitTests.Helpers
{
    /// <summary>
    /// Temporarily replaces the current culture and UI culture for the lifetime of the scope,
    /// restoring the previous values on disposal. Used to assert culture-independent behaviour
    /// without leaving process-wide state modified when a test fails.
    /// </summary>
    internal sealed class CultureScope : IDisposable
    {
        private readonly CultureInfo originalCulture;
        private readonly CultureInfo originalUICulture;
        private bool isDisposed;

        internal CultureScope(string cultureName)
            : this(new CultureInfo(cultureName))
        {
        }

        internal CultureScope(CultureInfo culture)
        {
            ArgumentNullException.ThrowIfNull(culture);

            originalCulture = Thread.CurrentThread.CurrentCulture;
            originalUICulture = Thread.CurrentThread.CurrentUICulture;

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        public void Dispose()
        {
            if (isDisposed)
            {
                return;
            }

            Thread.CurrentThread.CurrentCulture = originalCulture;
            Thread.CurrentThread.CurrentUICulture = originalUICulture;
            isDisposed = true;
        }
    }
}