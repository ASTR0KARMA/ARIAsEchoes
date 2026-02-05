using UnityEngine;

namespace NoaDebugger
{
    sealed class ApiLogOverlaySettingsGroupView : MutableSettingsViewBase
    {
        [SerializeField] EnumSettingsPanel _apiLogOverlayPosition;
        [SerializeField] FloatSettingsPanel _apiLogOverlayFontScale;
        [SerializeField] IntSettingsPanel _apiLogOverlayMaximumLogCount;
        [SerializeField] FloatSettingsPanel _apiLogOverlayMinimumOpacity;
        [SerializeField] FloatSettingsPanel _apiLogOverlayActiveDuration;
        [SerializeField] BoolSettingsPanel _apiLogOverlayShowTimestamp;
        [SerializeField] BoolSettingsPanel _apiLogOverlayShowMessageLogs;
        [SerializeField] BoolSettingsPanel _apiLogOverlayShowWarningLogs;
        [SerializeField] BoolSettingsPanel _apiLogOverlayShowErrorLogs;

        protected override SettingsResetButtonType ResetButtonType
        {
            get { return SettingsResetButtonType.ApiLogOverlay; }
        }

        public override void Initialize(SettingsViewLinker linker)
        {
            var settings = linker._apiLogOverlaySettings;
            _apiLogOverlayPosition.Initialize(settings.Position);
            _apiLogOverlayFontScale.Initialize(settings.FontScale);
            _apiLogOverlayMaximumLogCount.Initialize(settings.MaximumLogCount);
            _apiLogOverlayMinimumOpacity.Initialize(settings.MinimumOpacity);
            _apiLogOverlayActiveDuration.Initialize(settings.ActiveDuration);
            _apiLogOverlayShowTimestamp.Initialize(settings.ShowTimestamp);
            _apiLogOverlayShowMessageLogs.Initialize(settings.ShowMessageLogs);
            _apiLogOverlayShowWarningLogs.Initialize(settings.ShowWarningLogs);
            _apiLogOverlayShowErrorLogs.Initialize(settings.ShowErrorLogs);
        }
    }
}
