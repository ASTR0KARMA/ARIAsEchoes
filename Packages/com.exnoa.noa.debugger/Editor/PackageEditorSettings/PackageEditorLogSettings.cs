#if NOA_DEBUGGER
using UnityEditor;

namespace NoaDebugger
{
    sealed class PackageEditorLogSettings : PackageEditorSettingsBase
    {
        int _consoleLogCount;

        int _apiLogCount;

        public PackageEditorLogSettings(NoaDebuggerSettings settings) : base(settings) { }

        public override void ResetTmpDataWithSettings()
        {
            _consoleLogCount = _settings.ConsoleLogCount;
            _apiLogCount = _settings.ApiLogCount;
        }

        public override void ApplySettings()
        {
            _settings.ConsoleLogCount = _consoleLogCount;
            _settings.ApiLogCount = _apiLogCount;
        }

        public override void ResetDefault()
        {
            _consoleLogCount = NoaDebuggerDefine.DEFAULT_CONSOLE_LOG_COUNT;
            _apiLogCount = NoaDebuggerDefine.DEFAULT_API_LOG_COUNT;
        }

        public override void DrawGUI()
        {
            _DisplaySettingsCategoryHeader("Logs", ResetDefault);

            _consoleLogCount = EditorGUILayout.IntSlider(
                "Console log count",
                _consoleLogCount,
                NoaDebuggerDefine.ConsoleLogCountMin,
                NoaDebuggerDefine.ConsoleLogCountMax);

            _apiLogCount = EditorGUILayout.IntSlider(
                "API log count",
                _apiLogCount,
                NoaDebuggerDefine.ApiLogCountMin,
                NoaDebuggerDefine.ApiLogCountMax);
        }
    }
}
#endif
