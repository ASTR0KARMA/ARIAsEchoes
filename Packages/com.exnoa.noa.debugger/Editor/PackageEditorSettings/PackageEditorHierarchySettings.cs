#if NOA_DEBUGGER
using UnityEditor;

namespace NoaDebugger
{
    sealed class PackageEditorHierarchySettings : PackageEditorSettingsBase
    {
        int _hierarchyLevels;

        public PackageEditorHierarchySettings(NoaDebuggerSettings settings) : base(settings) { }

        public override void ResetTmpDataWithSettings()
        {
            _hierarchyLevels = _settings.HierarchyLevels;
        }

        public override void ApplySettings()
        {
            _settings.HierarchyLevels = _hierarchyLevels;
        }

        public override void ResetDefault()
        {
            _hierarchyLevels = NoaDebuggerDefine.DEFAULT_HIERARCHY_LEVELS;
        }

        public override void DrawGUI()
        {
            _DisplaySettingsCategoryHeader("Hierarchy", ResetDefault);

            _hierarchyLevels = EditorGUILayout.IntSlider(
                "Hierarchy levels",
                _hierarchyLevels,
                NoaDebuggerDefine.HierarchyLevelsMin,
                NoaDebuggerDefine.HierarchyLevelsMax);

            EditorBaseStyle.DrawUILine();
        }
    }
}
#endif
