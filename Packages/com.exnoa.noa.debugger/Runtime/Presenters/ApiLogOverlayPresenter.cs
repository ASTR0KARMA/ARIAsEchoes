namespace NoaDebugger
{
    sealed class ApiLogOverlayPresenter : LogOverlayPresenter
    {
        public ApiLogOverlayPresenter(LogOverlayView overlayPrefab, LogOverlaySettingsView overlaySettingsPrefab, string prefsKeyPrefix) : base(overlayPrefab, overlaySettingsPrefab, prefsKeyPrefix)
        {
            _overlayToolSettings = NoaDebuggerSettingsManager.GetCacheSettings<ApiLogOverlayRuntimeSettings>();
        }
    }
}
