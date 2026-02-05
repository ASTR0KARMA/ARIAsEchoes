using UnityEngine;

namespace NoaDebugger
{
    /// <summary>
    /// Represents a collection of the OS information.
    /// </summary>
    public sealed class OSInformationGroup
    {
        /// <summary>
        /// Gets the operating system name with version.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the operating system family the game is running on.
        /// </summary>
        public OperatingSystemFamily Family { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OSInformationGroup"/>.
        /// </summary>
        internal OSInformationGroup()
        {
            Name = SystemInfo.operatingSystem;
            Family = SystemInfo.operatingSystemFamily;
        }
    }
}
