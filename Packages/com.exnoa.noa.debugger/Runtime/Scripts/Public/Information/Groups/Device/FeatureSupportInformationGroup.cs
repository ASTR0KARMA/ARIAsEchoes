using UnityEngine;

namespace NoaDebugger
{
    /// <summary>
    /// Represents a collection of the feature support information.
    /// </summary>
    public sealed class FeatureSupportInformationGroup
    {
        /// <summary>
        /// True if there is an Audio device available for playback; otherwise false.
        /// </summary>
        public bool SupportsAudio { get; }

        /// <summary>
        /// True if an accelerometer is available on the device; otherwise false.
        /// </summary>
        public bool SupportsAccelerometer { get; }

        /// <summary>
        /// True if a gyroscope is available on the device; otherwise false.
        /// </summary>
        public bool SupportsGyroscope { get; }

        /// <summary>
        /// True if the device is capable of reporting its location; otherwise false.
        /// </summary>
        public bool SupportsLocationService { get; }

        /// <summary>
        /// True if the device is capable of providing the user haptic feedback by vibration; otherwise false.
        /// </summary>
        public bool SupportsVibration { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureSupportInformationGroup"/>.
        /// </summary>
        internal FeatureSupportInformationGroup()
        {
            SupportsAudio = SystemInfo.supportsAudio;
            SupportsAccelerometer = SystemInfo.supportsAccelerometer;
            SupportsGyroscope = SystemInfo.supportsGyroscope;
            SupportsLocationService = SystemInfo.supportsLocationService;
            SupportsVibration = SystemInfo.supportsVibration;
        }
    }
}
