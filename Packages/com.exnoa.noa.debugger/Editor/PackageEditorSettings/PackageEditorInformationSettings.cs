#if NOA_DEBUGGER
using UnityEditor;

namespace NoaDebugger
{
    sealed class PackageEditorInformationSettings : PackageEditorSettingsBase
    {
        bool _savesChanges;

        public PackageEditorInformationSettings(NoaDebuggerSettings settings) : base(settings) { }

        public override void ResetTmpDataWithSettings()
        {
            _savesChanges = _settings.SaveInformationValue;
        }

        public override void ApplySettings()
        {
            _settings.SaveInformationValue = _savesChanges;
        }

        public override void ResetDefault()
        {
            _savesChanges = NoaDebuggerDefine.DEFAULT_SAVE_INFORMATION_VALUE;
        }

        public override void DrawGUI()
        {
            _DisplaySettingsCategoryHeader("Information", ResetDefault);

            _savesChanges = EditorGUILayout.Toggle("Save changes", _savesChanges);
        }
    }
}
#endif
