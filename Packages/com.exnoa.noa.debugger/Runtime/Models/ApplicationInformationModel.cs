namespace NoaDebugger
{
    sealed class ApplicationInformationModel : ModelBase
    {
        public BuildInformationGroup Build { get; }

        public RuntimeInformationGroup Runtime { get; }

        public ScreenInformationGroup Screen { get; }

        public GraphicsSettingsInformationGroup GraphicsSettings { get; }

        public LoggingInformationGroup Logging { get; }

        public ApplicationOtherInformationGroup ApplicationOther { get; }

        public ApplicationInformationModel()
        {
            Build = new BuildInformationGroup();
            Runtime = new RuntimeInformationGroup();
            Screen = new ScreenInformationGroup();
            GraphicsSettings = new GraphicsSettingsInformationGroup();
            Logging = new LoggingInformationGroup();
            ApplicationOther = new ApplicationOtherInformationGroup();
        }

        public void OnUpdate()
        {
            Runtime.UpdateSettings();
            Screen.UpdateSettings();
            GraphicsSettings.UpdateSettings();
            Logging.UpdateSettings();
        }

        public IKeyValueParser[] CreateExportData()
        {
            return new IKeyValueParser[]
            {
                KeyObjectParser.CreateFromClass(Build, "Build"),
                KeyObjectParser.CreateFromClass(Runtime, "Runtime"),
                KeyObjectParser.CreateFromClass(Screen, "Screen"),
                KeyObjectParser.CreateFromClass(GraphicsSettings, "GraphicsSettings"),
                KeyObjectParser.CreateFromClass(Logging, "Logging"),
                KeyObjectParser.CreateFromClass(ApplicationOther, "Other")
            };
        }
    }
}
