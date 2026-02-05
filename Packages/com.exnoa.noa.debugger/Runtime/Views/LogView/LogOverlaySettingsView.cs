using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace NoaDebugger
{
    sealed class LogOverlaySettingsView : OverlaySettingsViewBase<LogOverlayBaseRuntimeSettings, OverlaySettingsViewLinker<LogOverlayBaseRuntimeSettings>>
    {
        enum FeatureType
        {
            ConsoleLog,
            ApiLog
        }

        [SerializeField]
        EnumSettingsPanel _position;
        [SerializeField]
        FloatSettingsPanel _fontScale;
        [SerializeField]
        IntSettingsPanel _maximumLogCount;
        [SerializeField]
        FloatSettingsPanel _minimumOpacity;
        [SerializeField]
        FloatSettingsPanel _activeDuration;
        [SerializeField]
        BoolSettingsPanel _showTimestamp;
        [SerializeField]
        BoolSettingsPanel _showMessageLogs;
        [SerializeField]
        BoolSettingsPanel _showWarningLogs;
        [SerializeField]
        BoolSettingsPanel _showErrorLogs;
        [SerializeField]
        FeatureType _type;

        void _OnValidateUI()
        {
            Assert.IsNotNull(_position);
            Assert.IsNotNull(_fontScale);
            Assert.IsNotNull(_activeDuration);
            Assert.IsNotNull(_minimumOpacity);
            Assert.IsNotNull(_maximumLogCount);
            Assert.IsNotNull(_showTimestamp);
            Assert.IsNotNull(_showMessageLogs);
            Assert.IsNotNull(_showWarningLogs);
            Assert.IsNotNull(_showErrorLogs);
        }

        protected override void _Init()
        {
            base._Init();
            _OnValidateUI();
        }

        protected override void _OnShow(OverlaySettingsViewLinker<LogOverlayBaseRuntimeSettings> linker)
        {
            base._OnShow(linker);

            var settings = linker.Settings;
            _position.Initialize(settings.Position);
            _fontScale.Initialize(settings.FontScale);
            _activeDuration.Initialize(settings.ActiveDuration);
            _minimumOpacity.Initialize(settings.MinimumOpacity);
            _maximumLogCount.Initialize(settings.MaximumLogCount);
            _showTimestamp.Initialize(settings.ShowTimestamp);
            _showMessageLogs.Initialize(settings.ShowMessageLogs);
            _showWarningLogs.Initialize(settings.ShowWarningLogs);
            _showErrorLogs.Initialize(settings.ShowErrorLogs);
        }

        protected override void Refresh()
        {
            _position.Refresh();
            _fontScale.Refresh();
            _activeDuration.Refresh();
            _minimumOpacity.Refresh();
            _maximumLogCount.Refresh();
            _showTimestamp.Refresh();
            _showMessageLogs.Refresh();
            _showWarningLogs.Refresh();
            _showErrorLogs.Refresh();
        }

        public override void SetActiveFooterArea()
        {
            bool isActive = _type switch
            {
                FeatureType.ConsoleLog => NoaDebuggerSettingsManager.HasUnsavedNoaDebuggerSettingsCache<ConsoleLogOverlayRuntimeSettings>(),
                FeatureType.ApiLog => NoaDebuggerSettingsManager.HasUnsavedNoaDebuggerSettingsCache<ApiLogOverlayRuntimeSettings>(),
                _ => NoaDebuggerSettingsManager.HasUnsavedNoaDebuggerSettingsCache<LogOverlayBaseRuntimeSettings>(),
            };
            _SetActiveFooterArea(isActive);
        }
    }
}
