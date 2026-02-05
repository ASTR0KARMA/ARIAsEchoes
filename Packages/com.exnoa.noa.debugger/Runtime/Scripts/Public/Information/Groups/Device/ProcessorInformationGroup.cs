using UnityEngine;

namespace NoaDebugger
{
    /// <summary>
    /// Represents a collection of the processor information.
    /// </summary>
    public sealed class ProcessorInformationGroup
    {
        /// <summary>
        /// Gets the processor name.
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// Gets the number of processors present.
        /// </summary>
        public int Count { get; }

        /// <summary>
        /// Gets the processor frequency in MHz.
        /// </summary>
        public int Frequency { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessorInformationGroup"/>.
        /// </summary>
        internal ProcessorInformationGroup()
        {
            Type = SystemInfo.processorType;
            Count = SystemInfo.processorCount;
            Frequency = SystemInfo.processorFrequency;
        }
    }
}
