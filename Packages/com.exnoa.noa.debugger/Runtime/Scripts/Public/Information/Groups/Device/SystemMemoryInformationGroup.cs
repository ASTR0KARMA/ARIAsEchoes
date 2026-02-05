using UnityEngine;

namespace NoaDebugger
{
    /// <summary>
    /// Represents a collection of the system memory information.
    /// </summary>
    public sealed class SystemMemoryInformationGroup
    {
        /// <summary>
        /// Gets the amount of system memory present.
        /// </summary>
        public int TotalSize { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemMemoryInformationGroup"/>.
        /// </summary>
        internal SystemMemoryInformationGroup()
        {
            TotalSize = SystemInfo.systemMemorySize;

        }
    }
}
