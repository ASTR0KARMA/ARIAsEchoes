using System;
using System.Globalization;
using UnityEngine;

namespace NoaDebugger
{
    /// <summary>
    /// Represents a collection of the system information.
    /// </summary>
    public sealed class SystemInformationGroup
    {
        /// <summary>
        /// Gets the language in which the user's operating system is running in.
        /// </summary>
        public SystemLanguage Language => Application.systemLanguage;

        /// <summary>
        /// Gets the <see cref="CultureInfo"/> object that represents the culture used by the current thread.
        /// </summary>
        public CultureInfo Region => CultureInfo.CurrentCulture;

        /// <summary>
        /// Gets a <see cref="TimeZoneInfo"/> object that represents the local time zone.
        /// </summary>
        public TimeZoneInfo TimeZone => TimeZoneInfo.Local;

        /// <summary>
        /// Gets a <see cref="LocalDateTime"/> object that is set to the current date and time on this computer, expressed as the local time.
        /// </summary>
        public DateTime LocalDateTime => DateTime.Now;
    }
}
