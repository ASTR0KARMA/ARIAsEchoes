#if NOA_DEBUGGER
using UnityEditor;

namespace NoaDebugger
{
    sealed class PackageEditorApiLogOverlaySettings : PackageEditorSettingsBase
    {
        NoaDebug.OverlayPosition _logOverlayPosition;

        float _logOverlayFontScale;

        int _logOverlayMaximumLogCount;

        float _logOverlayMinimumOpacity;

        float _logOverlayActiveDuration;

        bool _logOverlayShowTimestamp;

        bool _logOverlayShowMessageLogs;

        bool _logOverlayShowErrorLogs;

        public PackageEditorApiLogOverlaySettings(NoaDebuggerSettings settings) : base(settings) { }

        public override void ResetTmpDataWithSettings()
        {
            _logOverlayPosition = _settings.ApiLogOverlayPosition;
            _logOverlayFontScale = _settings.ApiLogOverlayFontScale;
            _logOverlayMaximumLogCount = _settings.ApiLogOverlayMaximumLogCount;
            _logOverlayMinimumOpacity = _settings.ApiLogOverlayMinimumOpacity;
            _logOverlayActiveDuration = _settings.ApiLogOverlayActiveDuration;
            _logOverlayShowTimestamp = _settings.ApiLogOverlayShowTimestamp;
            _logOverlayShowMessageLogs = _settings.ApiLogOverlayShowMessageLogs;
            _logOverlayShowErrorLogs = _settings.ApiLogOverlayShowErrorLogs;
        }

        public override void ApplySettings()
        {
            _settings.ApiLogOverlayPosition = _logOverlayPosition;
            _settings.ApiLogOverlayFontScale = _logOverlayFontScale;
            _settings.ApiLogOverlayMaximumLogCount = _logOverlayMaximumLogCount;
            _settings.ApiLogOverlayMinimumOpacity = _logOverlayMinimumOpacity;
            _settings.ApiLogOverlayActiveDuration = _logOverlayActiveDuration;
            _settings.ApiLogOverlayShowTimestamp = _logOverlayShowTimestamp;
            _settings.ApiLogOverlayShowMessageLogs = _logOverlayShowMessageLogs;
            _settings.ApiLogOverlayShowErrorLogs = _logOverlayShowErrorLogs;
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
            _logOverlayShowErrorLogs = NoaDebuggerDefine.DEFAULT_LOG_OVERLAY_SHOW_LOGS;
        }

        public override void DrawGUI()
        {
            _DisplaySettingsCategoryHeader("ApiLog", ResetDefault);

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
            _logOverlayShowErrorLogs = EditorGUILayout.Toggle("Show error logs", _logOverlayShowErrorLogs);
        }
    }
}
#endif
