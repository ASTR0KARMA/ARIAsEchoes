using UnityEngine;

namespace NoaDebugger
{
    /// <summary>
    /// Represents a collection of the device general information.
    /// </summary>
    public sealed class DeviceGeneralInformationGroup
    {
        /// <summary>
        /// Gets the model of device.
        /// </summary>
        public string Model { get; }

        /// <summary>
        /// Gets the kind of device the application is running on.
        /// </summary>
        public DeviceType Type { get; }

        /// <summary>
        /// Gets the user defined name of the device.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceGeneralInformationGroup"/>.
        /// </summary>
        internal DeviceGeneralInformationGroup()
        {
            Model = SystemInfo.deviceModel;
            Type = SystemInfo.deviceType;
            Name = SystemInfo.deviceName;
        }
    }
}
