#if NOA_DEBUGGER
using UnityEditor;

namespace NoaDebugger
{
    sealed class PackageEditorLoadingSettings : PackageEditorSettingsBase
    {
        bool _isAutoInitialize;

        public PackageEditorLoadingSettings(NoaDebuggerSettings settings) : base(settings) { }

        public override void ResetTmpDataWithSettings()
        {
            _isAutoInitialize = _settings.AutoInitialize;
        }

        public override void ApplySettings()
        {
            _settings.AutoInitialize = _isAutoInitialize;
        }

        public override void ResetDefault()
        {
            _isAutoInitialize = NoaDebuggerDefine.DEFAULT_AUTO_INITIALIZE;
        }

        public override void DrawGUI()
        {
            _DisplaySettingsCategoryHeader("Loading", ResetDefault);

            _isAutoInitialize = EditorGUILayout.Toggle("Automatically initialize", _isAutoInitialize);
        }
    }
}
#endif
