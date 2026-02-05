namespace NoaDebugger
{
    sealed class ConsoleLogOverlayPresenter : LogOverlayPresenter
    {
        public ConsoleLogOverlayPresenter(LogOverlayView overlayPrefab, LogOverlaySettingsView overlaySettingsPrefab, string prefsKeyPrefix) : base(overlayPrefab, overlaySettingsPrefab, prefsKeyPrefix)
        {
            _overlayToolSettings = NoaDebuggerSettingsManager.GetCacheSettings<ConsoleLogOverlayRuntimeSettings>();
        }
    }
}
