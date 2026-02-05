using UnityEngine;

namespace NoaDebugger
{
    sealed class ConsoleLogOverlaySettingsGroupView : MutableSettingsViewBase
    {
        [SerializeField] EnumSettingsPanel _consoleLogOverlayPosition;
        [SerializeField] FloatSettingsPanel _consoleLogOverlayFontScale;
        [SerializeField] IntSettingsPanel _consoleLogOverlayMaximumLogCount;
        [SerializeField] FloatSettingsPanel _consoleLogOverlayMinimumOpacity;
        [SerializeField] FloatSettingsPanel _consoleLogOverlayActiveDuration;
        [SerializeField] BoolSettingsPanel _consoleLogOverlayShowTimestamp;
        [SerializeField] BoolSettingsPanel _consoleLogOverlayShowMessageLogs;
        [SerializeField] BoolSettingsPanel _consoleLogOverlayShowWarningLogs;
        [SerializeField] BoolSettingsPanel _consoleLogOverlayShowErrorLogs;

        protected override SettingsResetButtonType ResetButtonType
        {
            get { return SettingsResetButtonType.ConsoleLogOverlay; }
        }

        public override void Initialize(SettingsViewLinker linker)
        {
            var settings = linker._consoleLogOverlaySettings;
            _consoleLogOverlayPosition.Initialize(settings.Position);
            _consoleLogOverlayFontScale.Initialize(settings.FontScale);
            _consoleLogOverlayMaximumLogCount.Initialize(settings.MaximumLogCount);
            _consoleLogOverlayMinimumOpacity.Initialize(settings.MinimumOpacity);
            _consoleLogOverlayActiveDuration.Initialize(settings.ActiveDuration);
            _consoleLogOverlayShowTimestamp.Initialize(settings.ShowTimestamp);
            _consoleLogOverlayShowMessageLogs.Initialize(settings.ShowMessageLogs);
            _consoleLogOverlayShowWarningLogs.Initialize(settings.ShowWarningLogs);
            _consoleLogOverlayShowErrorLogs.Initialize(settings.ShowErrorLogs);
        }
    }
}
