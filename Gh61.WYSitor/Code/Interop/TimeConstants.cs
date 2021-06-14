using System;

namespace Gh61.WYSitor.Code.Interop
{
    public static class TimeConstants
    {
        /// <summary>
        /// Minimal time between 2 events of HtmlContentChanged.
        /// </summary>
        public static readonly TimeSpan MinHtmlChangedInterval = TimeSpan.FromMilliseconds(500);

        /// <summary>
        /// Time between HtmlContentChanged events, when using fallback timer.
        /// </summary>
        public static readonly TimeSpan FallbackHtmlChangedInterval = TimeSpan.FromSeconds(1);
    }
}
