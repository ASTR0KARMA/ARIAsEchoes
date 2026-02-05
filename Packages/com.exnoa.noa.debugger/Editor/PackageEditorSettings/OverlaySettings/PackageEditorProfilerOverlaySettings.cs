#if NOA_DEBUGGER
using UnityEditor;

namespace NoaDebugger
{
    sealed class PackageEditorProfilerOverlaySettings : PackageEditorSettingsBase
    {
        bool _profilerOverlayFpsEnabled;

        NoaProfiler.OverlayTextType _profilerOverlayFpsTextType;

        bool _profilerOverlayFpsGraphVisibility;

        bool _profilerOverlayMemoryEnabled;

        NoaProfiler.OverlayTextType _profilerOverlayMemoryTextType;

        bool _profilerOverlayMemoryGraphVisibility;

        bool _profilerOverlayRenderingEnabled;

        NoaProfiler.OverlayTextType _profilerOverlayRenderingTextType;

        bool _profilerOverlayRenderingGraphVisibility;

        NoaDebug.OverlayPosition _profilerOverlayPosition;

        float _profilerOverlayScale;

        NoaProfiler.OverlayAxis _profilerOverlayAxis;

        public PackageEditorProfilerOverlaySettings(NoaDebuggerSettings settings) : base(settings) { }

        public override void ResetTmpDataWithSettings()
        {
            _profilerOverlayPosition  = _settings.ProfilerOverlayPosition ;
            _profilerOverlayScale = _settings.ProfilerOverlayScale;
            _profilerOverlayAxis = _settings.ProfilerOverlayAxis;

            _profilerOverlayFpsEnabled = _settings.ProfilerOverlayFpsSettings.Enabled;
            _profilerOverlayFpsTextType = _settings.ProfilerOverlayFpsSettings.TextType;
            _profilerOverlayFpsGraphVisibility = _settings.ProfilerOverlayFpsSettings.Graph;

            _profilerOverlayMemoryEnabled = _settings.ProfilerOverlayMemorySettings.Enabled;
            _profilerOverlayMemoryTextType = _settings.ProfilerOverlayMemorySettings.TextType;
            _profilerOverlayMemoryGraphVisibility = _settings.ProfilerOverlayMemorySettings.Graph;

            _profilerOverlayRenderingEnabled = _settings.ProfilerOverlayRenderingSettings.Enabled;
            _profilerOverlayRenderingTextType = _settings.ProfilerOverlayRenderingSettings.TextType;
            _profilerOverlayRenderingGraphVisibility = _settings.ProfilerOverlayRenderingSettings.Graph;
        }

        public override void ApplySettings()
        {
            _settings.ProfilerOverlayPosition  = _profilerOverlayPosition ;
            _settings.ProfilerOverlayScale = _profilerOverlayScale;
            _settings.ProfilerOverlayAxis = _profilerOverlayAxis;

            _settings.ProfilerOverlayFpsSettings.Enabled = _profilerOverlayFpsEnabled;
            _settings.ProfilerOverlayFpsSettings.TextType = _profilerOverlayFpsTextType;
            _settings.ProfilerOverlayFpsSettings.Graph = _profilerOverlayFpsGraphVisibility;

            _settings.ProfilerOverlayMemorySettings.Enabled = _profilerOverlayMemoryEnabled;
            _settings.ProfilerOverlayMemorySettings.TextType = _profilerOverlayMemoryTextType;
            _settings.ProfilerOverlayMemorySettings.Graph = _profilerOverlayMemoryGraphVisibility;

            _settings.ProfilerOverlayRenderingSettings.Enabled = _profilerOverlayRenderingEnabled;
            _settings.ProfilerOverlayRenderingSettings.TextType = _profilerOverlayRenderingTextType;
            _settings.ProfilerOverlayRenderingSettings.Graph = _profilerOverlayRenderingGraphVisibility;
        }

        public override void ResetDefault()
        {
            _profilerOverlayPosition = NoaDebuggerDefine.DEFAULT_PROFILER_OVERLAY_POSITION;
            _profilerOverlayScale = NoaDebuggerDefine.DEFAULT_PROFILER_OVERLAY_SCALE;
            _profilerOverlayAxis = NoaDebuggerDefine.DEFAULT_PROFILER_OVERLAY_AXIS;

            _profilerOverlayFpsEnabled = NoaDebuggerDefine.DefaultProfilerOverlayFeatureEnabled;
            _profilerOverlayFpsTextType = NoaDebuggerDefine.DefaultProfilerOverlayTextType;
            _profilerOverlayFpsGraphVisibility = NoaDebuggerDefine.DefaultProfilerOverlayGraphEnabled;

            _profilerOverlayMemoryEnabled = NoaDebuggerDefine.DefaultProfilerOverlayFeatureEnabled;
            _profilerOverlayMemoryTextType = NoaDebuggerDefine.DefaultProfilerOverlayTextType;
            _profilerOverlayMemoryGraphVisibility = NoaDebuggerDefine.DefaultProfilerOverlayGraphEnabled;

            _profilerOverlayRenderingEnabled = NoaDebuggerDefine.DefaultProfilerOverlayFeatureEnabled;
            _profilerOverlayRenderingTextType = NoaDebuggerDefine.DefaultProfilerOverlayTextType;
            _profilerOverlayRenderingGraphVisibility = NoaDebuggerDefine.DefaultProfilerOverlayGraphEnabled;
        }

        public override void DrawGUI()
        {
            _DisplaySettingsCategoryHeader("Profiler", ResetDefault);

            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.LabelField($"<b>FPS</b>", EditorBaseStyle.Headline());
                _profilerOverlayFpsEnabled = EditorGUILayout.Toggle("Enabled", _profilerOverlayFpsEnabled);
                _profilerOverlayFpsTextType = (NoaProfiler.OverlayTextType)EditorGUILayout.EnumPopup("Text", _profilerOverlayFpsTextType);
                _profilerOverlayFpsGraphVisibility = EditorGUILayout.Toggle("Graph", _profilerOverlayFpsGraphVisibility);
                EditorGUILayout.Separator();

                EditorGUILayout.LabelField($"<b>Memory</b>", EditorBaseStyle.Headline());
                _profilerOverlayMemoryEnabled = EditorGUILayout.Toggle("Enabled", _profilerOverlayMemoryEnabled);
                _profilerOverlayMemoryTextType = (NoaProfiler.OverlayTextType)EditorGUILayout.EnumPopup("Text", _profilerOverlayMemoryTextType);
                _profilerOverlayMemoryGraphVisibility = EditorGUILayout.Toggle("Graph", _profilerOverlayMemoryGraphVisibility);
                EditorGUILayout.Separator();

                EditorGUILayout.LabelField($"<b>Rendering</b>", EditorBaseStyle.Headline());
                _profilerOverlayRenderingEnabled = EditorGUILayout.Toggle("Enabled", _profilerOverlayRenderingEnabled);
                _profilerOverlayRenderingTextType = (NoaProfiler.OverlayTextType)EditorGUILayout.EnumPopup("Text", _profilerOverlayRenderingTextType);
                _profilerOverlayRenderingGraphVisibility = EditorGUILayout.Toggle("Graph", _profilerOverlayRenderingGraphVisibility);
                EditorGUILayout.Separator();

                EditorGUILayout.LabelField($"<b>Other</b>", EditorBaseStyle.Headline());
                _profilerOverlayPosition =
                    (NoaDebug.OverlayPosition)EditorGUILayout.EnumPopup("Position", _profilerOverlayPosition);

                _profilerOverlayScale = EditorGUILayout.Slider(
                    "Scale",
                    _profilerOverlayScale,
                    NoaDebuggerDefine.ProfilerOverlayScaleMin,
                    NoaDebuggerDefine.ProfilerOverlayScaleMax);

                _profilerOverlayAxis = (NoaProfiler.OverlayAxis)EditorGUILayout.EnumPopup("Axis", _profilerOverlayAxis);
            }
        }
    }
}
#endif
