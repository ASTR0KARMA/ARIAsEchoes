namespace NoaDebugger
{
    abstract class LogOverlaySettingsDefaultGetter
    {
        protected readonly NoaDebuggerSettings _settings;

        protected LogOverlaySettingsDefaultGetter()
        {
            _settings = NoaDebuggerSettingsManager.GetNoaDebuggerSettings();
        }

        public abstract NoaDebug.OverlayPosition DefaultPosition { get; }

        public abstract float DefaultFontScale { get; }

        public abstract int DefaultMaximumLogCount { get; }

        public abstract float DefaultMinimumOpacity { get; }

        public abstract float DefaultActiveDuration { get; }

        public abstract bool DefaultShowTimestamp { get; }

        public abstract bool DefaultShowMessageLogs { get; }

        public abstract bool DefaultShowWarningLogs { get; }

        public abstract bool DefaultShowErrorLogs { get; }
    }
}
