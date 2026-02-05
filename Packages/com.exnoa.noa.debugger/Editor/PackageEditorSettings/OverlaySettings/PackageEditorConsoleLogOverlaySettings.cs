#if NOA_DEBUGGER
using UnityEditor;

namespace NoaDebugger
{
    sealed class PackageEditorConsoleLogOverlaySettings : PackageEditorSettingsBase
    {
        NoaDebug.OverlayPosition _logOverlayPosition;

        float _logOverlayFontScale;

        int _logOverlayMaximumLogCount;

        float _logOverlayMinimumOpacity;

        float _logOverlayActiveDuration;

        bool _logOverlayShowTimestamp;

        bool _logOverlayShowMessageLogs;

        bool _logOverlayShowWarningLogs;

        bool _logOverlayShowErrorLogs;

        public PackageEditorConsoleLogOverlaySettings(NoaDebuggerSettings settings) : base(settings) { }

        public override void ResetTmpDataWithSettings()
        {
            _logOverlayPosition = _settings.ConsoleLogOverlayPosition;
            _logOverlayFontScale = _settings.ConsoleLogOverlayFontScale;
            _logOverlayMaximumLogCount = _settings.ConsoleLogOverlayMaximumLogCount;
            _logOverlayMinimumOpacity = _settings.ConsoleLogOverlayMinimumOpacity;
            _logOverlayActiveDuration = _settings.ConsoleLogOverlayActiveDuration;
            _logOverlayShowTimestamp = _settings.ConsoleLogOverlayShowTimestamp;
            _logOverlayShowMessageLogs = _settings.ConsoleLogOverlayShowMessageLogs;
            _logOverlayShowWarningLogs = _settings.ConsoleLogOverlayShowWarningLogs;
            _logOverlayShowErrorLogs = _settings.ConsoleLogOverlayShowErrorLogs;
        }

        public override void ApplySettings()
        {
            _settings.ConsoleLogOverlayPosition = _logOverlayPosition;
            _settings.ConsoleLogOverlayFontScale = _logOverlayFontScale;
            _settings.ConsoleLogOverlayMaximumLogCount = _logOverlayMaximumLogCount;
            _settings.ConsoleLogOverlayMinimumOpacity = _logOverlayMinimumOpacity;
            _settings.ConsoleLogOverlayActiveDuration = _logOverlayActiveDuration;
            _settings.ConsoleLogOverlayShowTimestamp = _logOverlayShowTimestamp;
            _settings.ConsoleLogOverlayShowMessageLogs = _logOverlayShowMessageLogs;
            _settings.ConsoleLogOverlayShowWarningLogs = _logOverlayShowWarningLogs;
            _settings.ConsoleLogOverlayShowErrorLogs = _logOverlayShowErrorLogs;
        }

        public override void ResetDefault()
        {
            _logOverlayPosition = NoaDebuggerDefine.DEFAULT_LOG_OVERLAY_POSITION;
            _logOverlayFontScale = NoaDebuggerDefine.DEFAULT_LOG_OVERLAY_FONT_SCALE;
            _logOverlayMaximumLogCount = NoaDebuggerDefine.DEFAULT_LOG_OVERLAY_MAXIMUM_LOG_COUNT;
            _logOverlayMinimumOpacity = NoaDebuggerDefine.DEFAULT_LOG_OVERLAY_MINIMUM_OPACITY;
            _logOverlayActiveDuration = NoaDebuggerDefine.DEFAULT_LOG_OVERLAY_ACTIVE_DURATION;
            _logOverlayShowTimestamp = NoaDebuggerDefine.DEFAULT_LOG_OVERLAY_SHOW_TIMESTAMP;
            _logOverlayShowMessageLogs = NoaDebuggerDefine.DEFAULT_LOG_OVERLAY_SHOW_LOGS;
            _logOverlayShowWarningLogs = NoaDebuggerDefine.DEFAULT_LOG_OVERLAY_SHOW_LOGS;
            _logOverlayShowErrorLogs = NoaDebuggerDefine.DEFAULT_LOG_OVERLAY_SHOW_LOGS;
        }

        public override void DrawGUI()
        {
            _DisplaySettingsCategoryHeader("ConsoleLog", ResetDefault);

            _logOverlayPosition =  (NoaDebug.OverlayPosition)EditorGUILayout.EnumPopup("Position", _logOverlayPosition);
            _logOverlayFontScale = EditorGUILayout.Slider(
                "Font scale",
                _logOverlayFontScale,
                NoaDebuggerDefine.LogOverlayFontScaleMin,
                NoaDebuggerDefine.LogOverlayFontScaleMax);
            _logOverlayMaximumLogCount = EditorGUILayout.IntSlider(
                "Maximum log count",
                _logOverlayMaximumLogCount,
                NoaDebuggerDefine.LogOverlayMaximumLogCountMin,
                NoaDebuggerDefine.LogOverlayMaximumLogCountMax);
            _logOverlayMinimumOpacity = EditorGUILayout.Slider(
                "Minimum opacity",
                _logOverlayMinimumOpacity,
                NoaDebuggerDefine.LogOverlayMinimumOpacityMin,
                NoaDebuggerDefine.LogOverlayMinimumOpacityMax);
            _logOverlayActiveDuration = EditorGUILayout.Slider(
                "Active duration",
                _logOverlayActiveDuration,
                NoaDebuggerDefine.LogOverlayActiveDurationMin,
                NoaDebuggerDefine.LogOverlayActiveDurationMax);
            _logOverlayShowTimestamp = EditorGUILayout.Toggle("Show timestamp", _logOverlayShowTimestamp);
            _logOverlayShowMessageLogs = EditorGUILayout.Toggle("Show message logs", _logOverlayShowMessageLogs);
            _logOverlayShowWarningLogs = EditorGUILayout.Toggle("Show warning logs", _logOverlayShowWarningLogs);
            _logOverlayShowErrorLogs = EditorGUILayout.Toggle("Show error logs", _logOverlayShowErrorLogs);
        }
    }
}
#endif
