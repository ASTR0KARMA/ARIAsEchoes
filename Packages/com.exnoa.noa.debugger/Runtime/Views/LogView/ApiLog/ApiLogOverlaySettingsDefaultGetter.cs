namespace NoaDebugger
{
    sealed class ApiLogOverlaySettingsDefaultGetter : LogOverlaySettingsDefaultGetter
    {
        public ApiLogOverlaySettingsDefaultGetter() : base() { }

        public override NoaDebug.OverlayPosition DefaultPosition => _settings.ApiLogOverlayPosition;
        public override float DefaultFontScale => _settings.ApiLogOverlayFontScale;
        public override int DefaultMaximumLogCount => _settings.ApiLogOverlayMaximumLogCount;
        public override float DefaultMinimumOpacity => _settings.ApiLogOverlayMinimumOpacity;
        public override float DefaultActiveDuration => _settings.ApiLogOverlayActiveDuration;
        public override bool DefaultShowTimestamp => _settings.ApiLogOverlayShowTimestamp;
        public override bool DefaultShowMessageLogs => _settings.ApiLogOverlayShowMessageLogs;
        public override bool DefaultShowWarningLogs => false; 
        public override bool DefaultShowErrorLogs => _settings.ApiLogOverlayShowErrorLogs;
    }
}
