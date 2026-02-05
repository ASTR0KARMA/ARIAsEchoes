namespace NoaDebugger
{
    sealed class ConsoleLogOverlaySettingsDefaultGetter : LogOverlaySettingsDefaultGetter
    {
        public ConsoleLogOverlaySettingsDefaultGetter() : base() { }

        public override NoaDebug.OverlayPosition DefaultPosition => _settings.ConsoleLogOverlayPosition;
        public override float DefaultFontScale => _settings.ConsoleLogOverlayFontScale;
        public override int DefaultMaximumLogCount => _settings.ConsoleLogOverlayMaximumLogCount;
        public override float DefaultMinimumOpacity => _settings.ConsoleLogOverlayMinimumOpacity;
        public override float DefaultActiveDuration => _settings.ConsoleLogOverlayActiveDuration;
        public override bool DefaultShowTimestamp => _settings.ConsoleLogOverlayShowTimestamp;
        public override bool DefaultShowMessageLogs => _settings.ConsoleLogOverlayShowMessageLogs;
        public override bool DefaultShowWarningLogs => _settings.ConsoleLogOverlayShowWarningLogs;
        public override bool DefaultShowErrorLogs => _settings.ConsoleLogOverlayShowErrorLogs;
    }
}
