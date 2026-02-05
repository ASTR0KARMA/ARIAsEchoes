using UnityEngine;
using UnityEngine.Rendering;

namespace NoaDebugger
{
    /// <summary>
    /// Represents a collection of the graphics device information.
    /// </summary>
    public sealed class GraphicsDeviceInformationGroup
    {
        /// <summary>
        /// Gets the name of the graphics device.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the graphics API type and driver version used by the graphics device.
        /// </summary>
        public string Version { get; }

        /// <summary>
        /// Gets the graphics API type used by the graphics device.
        /// </summary>
        public GraphicsDeviceType Type { get; }

        /// <summary>
        /// Gets the vendor of the graphics device.
        /// </summary>
        public string Vendor { get; }

        /// <summary>
        /// Gets the amount of video memory present.
        /// </summary>
        public int MemorySize { get; }

        /// <summary>
        /// True if the graphics device is using multi-threaded rendering; otherwise false.
        /// </summary>
        public bool MultiThreaded { get; }

        /// <summary>
        /// Gets the graphics device shader capability level.
        /// </summary>
        public int ShaderLevel { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsDeviceInformationGroup"/>.
        /// </summary>
        internal GraphicsDeviceInformationGroup()
        {
            Name = SystemInfo.graphicsDeviceName;
            Version = SystemInfo.graphicsDeviceVersion;
            Type = SystemInfo.graphicsDeviceType;
            Vendor = SystemInfo.graphicsDeviceVendor;
            MemorySize = SystemInfo.graphicsMemorySize;
            MultiThreaded = SystemInfo.graphicsMultiThreaded;
            ShaderLevel = SystemInfo.graphicsShaderLevel;
        }
    }
}
