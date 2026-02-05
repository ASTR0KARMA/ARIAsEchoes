namespace NoaDebugger
{
    sealed class DeviceInformationModel : ModelBase
    {
        public DeviceGeneralInformationGroup DeviceGeneral { get; }

        public OSInformationGroup OS { get; }

        public ProcessorInformationGroup Processor { get; }

        public GraphicsDeviceInformationGroup GraphicsDevice { get; }

        public SystemMemoryInformationGroup SystemMemory { get; }

        public DisplayInformationGroup Display { get; }

        public GraphicsSupportInformationGroup GraphicsSupport { get; }

        public TextureFormatSupportInformationGroup TextureFormatSupport { get; }

        public FeatureSupportInformationGroup FeatureSupport { get; }

        public NetworkInformationGroup Network { get; }

        public SystemInformationGroup System { get; }

        public InputInformationGroup Input { get; }

        public DeviceInformationModel()
        {
            DeviceGeneral = new DeviceGeneralInformationGroup();
            OS = new OSInformationGroup();
            Processor = new ProcessorInformationGroup();
            GraphicsDevice = new GraphicsDeviceInformationGroup();
            SystemMemory = new SystemMemoryInformationGroup();
            Display = new DisplayInformationGroup();
            GraphicsSupport = new GraphicsSupportInformationGroup();
            TextureFormatSupport = new TextureFormatSupportInformationGroup();
            FeatureSupport = new FeatureSupportInformationGroup();
            Network = new NetworkInformationGroup();
            System = new SystemInformationGroup();
            Input = new InputInformationGroup();
        }

        public void OnUpdate()
        {
        }

        public IKeyValueParser[] CreateExportData()
        {
            return new IKeyValueParser[]
            {
                KeyObjectParser.CreateFromClass(DeviceGeneral, "General"),
                KeyObjectParser.CreateFromClass(OS, "OS"),
                KeyObjectParser.CreateFromClass(Processor, "Processor"),
                KeyObjectParser.CreateFromClass(GraphicsDevice, "GraphicsDevice"),
                KeyObjectParser.CreateFromClass(SystemMemory, "SystemMemory"),
                KeyObjectParser.CreateFromClass(Display, "Display"),
                KeyObjectParser.CreateFromClass(GraphicsSupport, "GraphicsSupport"),
                KeyObjectParser.CreateFromClass(TextureFormatSupport, "TextureFormatSupport"),
                KeyObjectParser.CreateFromClass(FeatureSupport, "FeatureSupport"),
                KeyObjectParser.CreateFromClass(Network, "Network"),
                KeyObjectParser.CreateFromClass(System, "System"),
                KeyObjectParser.CreateFromClass(Input, "Input")
            };
        }
    }
}
