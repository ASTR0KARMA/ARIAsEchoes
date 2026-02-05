using UnityEngine;

namespace NoaDebugger
{
    sealed class NoaDebuggerOverlaySettings : NoaDebuggerSettingsBase
    {
        public NoaDebuggerOverlaySettings(NoaDebuggerSettings settings) : base(settings) { }

        public override void Init()
        {
            _settings.OverlayBackgroundOpacity = NoaDebuggerDefine.DEFAULT_OVERLAY_BACKGROUND_OPACITY;
            _settings.AppliesOverlaySafeArea = NoaDebuggerDefine.DEFAULT_OVERLAY_SAFE_AREA_APPLICABLE;
            _settings.OverlayPadding = new Vector2(NoaDebuggerDefine.DEFAULT_OVERLAY_PADDING_X, NoaDebuggerDefine.DEFAULT_OVERLAY_PADDING_Y);

            _settings.ProfilerOverlayPosition = NoaDebuggerDefine.DEFAULT_PROFILER_OVERLAY_POSITION;
            _settings.ProfilerOverlayScale = NoaDebuggerDefine.DEFAULT_PROFILER_OVERLAY_SCALE;
            _settings.ProfilerOverlayAxis = NoaDebuggerDefine.DEFAULT_PROFILER_OVERLAY_AXIS;

            _settings.ProfilerOverlayFpsSettings = new ProfilerOverlayFeatureSettings();
            _settings.ProfilerOverlayFpsSettings.Enabled = NoaDebuggerDefine.DefaultProfilerOverlayFeatureEnabled;
            _settings.ProfilerOverlayFpsSettings.TextType = NoaDebuggerDefine.DefaultProfilerOverlayTextType;
            _settings.ProfilerOverlayFpsSettings.Graph = NoaDebuggerDefine.DefaultProfilerOverlayGraphEnabled;

            _settings.ProfilerOverlayMemorySettings = new ProfilerOverlayFeatureSettings();
            _settings.ProfilerOverlayMemorySettings.Enabled = NoaDebuggerDefine.DefaultProfilerOverlayFeatureEnabled;
            _settings.ProfilerOverlayMemorySettings.TextType = NoaDebuggerDefine.DefaultProfilerOverlayTextType;
            _settings.ProfilerOverlayMemorySettings.Graph = NoaDebuggerDefine.DefaultProfilerOverlayGraphEnabled;

            _settings.ProfilerOverlayRenderingSettings = new ProfilerOverlayFeatureSettings();
            _settings.ProfilerOverlayRenderingSettings.Enabled = NoaDebuggerDefine.DefaultProfilerOverlayFeatureEnabled;
            _settings.ProfilerOverlayRenderingSettings.TextType = NoaDebuggerDefine.DefaultProfilerOverlayTextType;
            _settings.ProfilerOverlayRenderingSettings.Graph = NoaDebuggerDefine.DefaultProfilerOverlayGraphEnabled;

            _settings.ConsoleLogOverlayPosition = NoaDebuggerDefine.DEFAULT_LOG_OVERLAY_POSITION;
            _settings.ConsoleLogOverlayFontScale = NoaDebuggerDefine.DEFAULT_LOG_OVERLAY_FONT_SCALE;
            _settings.ConsoleLogOverlayMaximumLogCount = NoaDebuggerDefine.DEFAULT_LOG_OVERLAY_MAXIMUM_LOG_COUNT;
            _settings.ConsoleLogOverlayMinimumOpacity = NoaDebuggerDefine.DEFAULT_LOG_OVERLAY_MINIMUM_OPACITY;
            _settings.ConsoleLogOverlayActiveDuration = NoaDebuggerDefine.DEFAULT_LOG_OVERLAY_ACTIVE_DURATION;
            _settings.ConsoleLogOverlayShowTimestamp = NoaDebuggerDefine.DEFAULT_LOG_OVERLAY_SHOW_TIMESTAMP;
            _settings.ConsoleLogOverlayShowMessageLogs = NoaDebuggerDefine.DEFAULT_LOG_OVERLAY_SHOW_LOGS;
            _settings.ConsoleLogOverlayShowWarningLogs = NoaDebuggerDefine.DEFAULT_LOG_OVERLAY_SHOW_LOGS;
            _settings.ConsoleLogOverlayShowErrorLogs = NoaDebuggerDefine.DEFAULT_LOG_OVERLAY_SHOW_LOGS;

            _settings.ApiLogOverlayPosition = NoaDebuggerDefine.DEFAULT_LOG_OVERLAY_POSITION;
            _settings.ApiLogOverlayFontScale = NoaDebuggerDefine.DEFAULT_LOG_OVERLAY_FONT_SCALE;
            _settings.ApiLogOverlayMaximumLogCount = NoaDebuggerDefine.DEFAULT_LOG_OVERLAY_MAXIMUM_LOG_COUNT;
            _settings.ApiLogOverlayMinimumOpacity = NoaDebuggerDefine.DEFAULT_LOG_OVERLAY_MINIMUM_OPACITY;
            _settings.ApiLogOverlayActiveDuration = NoaDebuggerDefine.DEFAULT_LOG_OVERLAY_ACTIVE_DURATION;
            _settings.ApiLogOverlayShowTimestamp = NoaDebuggerDefine.DEFAULT_LOG_OVERLAY_SHOW_TIMESTAMP;
            _settings.ApiLogOverlayShowMessageLogs = NoaDebuggerDefine.DEFAULT_LOG_OVERLAY_SHOW_LOGS;
            _settings.ApiLogOverlayShowErrorLogs = NoaDebuggerDefine.DEFAULT_LOG_OVERLAY_SHOW_LOGS;
        }
    }
}
