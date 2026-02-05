namespace NoaDebugger
{
    /// <summary>
    /// Represents a collection of the application information.
    /// </summary>
    public sealed class ApplicationInformation
    {
        /// <summary>
        /// Gets the collection of the build information.
        /// </summary>
        public BuildInformationGroup Build { get; }

        /// <summary>
        /// Gets the collection of the runtime information.
        /// </summary>
        public RuntimeInformationGroup Runtime { get; }

        /// <summary>
        /// Gets the collection of the screen information.
        /// </summary>
        public ScreenInformationGroup Screen { get; }

        /// <summary>
        /// Gets the collection of the graphics settings information.
        /// </summary>
        public GraphicsSettingsInformationGroup GraphicsSettings { get; }

        /// <summary>
        /// Gets the collection of the logging information.
        /// </summary>
        public LoggingInformationGroup Logging { get; }

        /// <summary>
        /// Gets the collection of the other information for application.
        /// </summary>
        public ApplicationOtherInformationGroup ApplicationOther { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationInformation"/>.
        /// </summary>
        internal ApplicationInformation(ApplicationInformationModel model)
        {
            Build = model.Build;
            Runtime = model.Runtime;
            Screen = model.Screen;
            GraphicsSettings = model.GraphicsSettings;
            Logging = model.Logging;
            ApplicationOther = model.ApplicationOther;
        }
    }
}
