namespace NoaDebugger
{
    /// <summary>
    /// Represents a collection of the device information.
    /// </summary>
    public sealed class DeviceInformation
    {
        /// <summary>
        /// Gets the collection of the device general information.
        /// </summary>
        public DeviceGeneralInformationGroup DeviceGeneral { get; }

        /// <summary>
        /// Gets the collection of the OS information.
        /// </summary>
        public OSInformationGroup OS { get; }

        /// <summary>
        /// Gets the collection of the processor information.
        /// </summary>
        public ProcessorInformationGroup Processor { get; }

        /// <summary>
        /// Gets the collection of the graphics device information.
        /// </summary>
        public GraphicsDeviceInformationGroup GraphicsDevice { get; }

        /// <summary>
        /// Gets the collection of the system memory information.
        /// </summary>
        public SystemMemoryInformationGroup SystemMemory { get; }

        /// <summary>
        /// Gets the collection of the display information.
        /// </summary>
        public DisplayInformationGroup Display { get; }

        /// <summary>
        /// Gets the collection of the graphics support information.
        /// </summary>
        public GraphicsSupportInformationGroup GraphicsSupport { get; }

        /// <summary>
        /// Gets the collection of the texture format support information.
        /// </summary>
        public TextureFormatSupportInformationGroup TextureFormatSupport { get; }

        /// <summary>
        /// Gets the collection of the feature support information.
        /// </summary>
        public FeatureSupportInformationGroup FeatureSupport { get; }

        /// <summary>
        /// Gets the collection of the network information.
        /// </summary>
        public NetworkInformationGroup Network { get; }

        /// <summary>
        /// Gets the collection of the system information.
        /// </summary>
        public SystemInformationGroup System { get; }

        /// <summary>
        /// Gets the collection of the input information.
        /// </summary>
        public InputInformationGroup Input { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceInformation"/>.
        /// </summary>
        internal DeviceInformation(DeviceInformationModel model)
        {
            DeviceGeneral = model.DeviceGeneral;
            OS = model.OS;
            Processor = model.Processor;
            GraphicsDevice = model.GraphicsDevice;
            SystemMemory = model.SystemMemory;
            Display = model.Display;
            GraphicsSupport = model.GraphicsSupport;
            TextureFormatSupport = model.TextureFormatSupport;
            FeatureSupport = model.FeatureSupport;
            Network = model.Network;
            System = model.System;
            Input = model.Input;
        }
    }
}
