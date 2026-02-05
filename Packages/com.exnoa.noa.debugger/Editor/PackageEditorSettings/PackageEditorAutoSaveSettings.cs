#if NOA_DEBUGGER
using UnityEditor;

namespace NoaDebugger
{
    sealed class PackageEditorAutoSaveSettings : PackageEditorSettingsBase
    {
        public bool IsAutoSave => _isAutoSave;
        bool _isAutoSave;

        public PackageEditorAutoSaveSettings(NoaDebuggerSettings settings) : base(settings) { }

        public override void ResetTmpDataWithSettings()
        {
            _isAutoSave = _settings.AutoSave;
        }

        public override void ApplySettings()
        {
            _settings.AutoSave = _isAutoSave;
        }

        public override void ResetDefault()
        {
        }

        public override void DrawGUI()
        {
            _isAutoSave = EditorGUILayout.ToggleLeft("Auto save", _isAutoSave);
        }
    }
}
#endif

